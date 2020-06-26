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
using Confluent.Kafka;
using Serilog.Core;

namespace microsrv
{
    public class Reader
    {
        static ConsumerConfig consumerConfig = null;

        public static void ReadMessage(Logger log, string hostNameAndPort) {
            consumerConfig = new ConsumerConfig {
                BootstrapServers = hostNameAndPort,
                GroupId = "microsrv",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_,e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            try {
                log.Information($"Connecting at {hostNameAndPort} ...");
                using (var csm = new ConsumerBuilder<Ignore, string>(consumerConfig).Build()) {
                    log.Information("Connected. Starting reading messages. CTRL+C to quit.");
                    csm.Subscribe("microsrv");
                    try
                    {
                        while(true) {
                            var cs = csm.Consume(cts.Token);
                            log.Information($"Mensagem: {cs.Message.Value}");
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        csm.Close();
                        Console.WriteLine("Canceled by user");
                    }                    
                }
            }
            catch (Exception e) {
                log.Error($"Erro: {e.GetType().FullName} - {e.Message}");
            }
        }
    }
}