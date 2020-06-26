/*
Copyright (c) 2020, George L. Maluf.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using System;
using System.Threading;
using Serilog;
namespace microsrv
{
    class Program
    {
        static void Main(string[] args)
        {
            AutoResetEvent _closing = new AutoResetEvent(false);

            var logger = new LoggerConfiguration()
                            .WriteTo
                            .Console()
                            .CreateLogger();

            if (args.Length != 2) {
                WriteHelpUsage();
                return;
            }

            switch (args[0].ToUpper())
            {
                case "--P":
                    logger.Information("Starting as publisher");
                    Publisher.SendMessage(logger, args[1]);
                    Console.CancelKeyPress += ( _, e) => {
                        Console.WriteLine("Canceled by user.");
                        _closing.Set();
                    };
                    _closing.WaitOne();
                    break;
                case "--R":
                    logger.Information("Starting as reader");
                    Reader.ReadMessage(logger, args[1]);
                    break;
                default:
                    WriteHelpUsage();
                    return;
            }
        }

        private static void WriteHelpUsage() {
            Console.WriteLine("microsrv - A small microservice written in C# with .NET Core framework");
            Console.WriteLine($"Version : {typeof(Program).Assembly.GetName().Version}" );
            Console.WriteLine("Copyright (c) 2020, George L. Maluf.");                        
            Console.WriteLine("microsrv {--p|--c} <hostname>:<port>");
            Console.WriteLine(" ");
            Console.WriteLine("Options");
            Console.WriteLine("\t--p");
            Console.WriteLine("\tThe microservice acts a publisher, sending a sample message to hostname and port");
            Console.WriteLine("\tevery 5 seconds ");
            Console.WriteLine(" ");
            Console.WriteLine("\t--r");
            Console.WriteLine("\tThe microservice display the messages readed from hostname and port");
            Console.WriteLine(" ");
            Console.WriteLine("\t<hostname>:<port>");
            Console.WriteLine("\tA valid hostname / IP number and port to send or listen a message");
            Console.WriteLine(" ");
            Console.WriteLine("THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS \"AS IS\" AND");
            Console.WriteLine("ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED");
            Console.WriteLine("WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE");
            Console.WriteLine("DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR");
            Console.WriteLine("ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES");
            Console.WriteLine("(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;");
            Console.WriteLine("LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND");
            Console.WriteLine("ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT");
            Console.WriteLine("(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS");
            Console.WriteLine("SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.");
        }
        // static void OnExit(object sender, ConsoleCancelEventArgs args)
        // {
        //     Console.WriteLine("Exit");
        //         _closing.Set();
        // }
    }
}
