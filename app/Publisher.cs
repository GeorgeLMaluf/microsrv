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
    public class Publisher
    {
        static ProducerConfig producerConfig = null;

        public static async void SendMessage(Logger log, string hostNameAndPort) {

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => {
                e.Cancel = true;
                cts.Cancel();
            };

            producerConfig = new ProducerConfig {
                BootstrapServers = "localhost:9092"
            };
            try {
                log.Information($"Connecting at {hostNameAndPort} ...");
                using (var producer = new ProducerBuilder<string,string>(producerConfig).Build()) {
                    log.Information("Connected. Starting sending messages. CTRL+C to quit.");
                    try {
                        while (true) {
                            var line = new MessageLine();
                            var msg = new Message<string, string> {
                                Key = null,
                                Value = line.ToString()
                            };
                            var resp = await producer.ProduceAsync("microsrv", msg, cts.Token);
                            log.Information($"MsgId: {line.Id.ToString()} - Status:{resp.Status.ToString()}");
                            System.Threading.Thread.Sleep(5000);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        producer.Flush();
                        log.Warning("Operation canceled.");
                    }
                }                
            }
            catch (Exception e) {
                log.Error($"Exception: {e.GetType().FullName} - Message: {e.Message}");
            }
        }
    }
}