//using aspcore_async_deploy_smart_contract.Contract.Repository;
//using aspcore_async_deploy_smart_contract.Dal.Entities;
//using Moq;
//using System;
//using System.Threading.Tasks;

//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.DependencyInjection;

//using Xunit;
//using Xunit.Abstractions;

//using Nethereum.RPC.Eth.DTOs;
//using aspcore_async_deploy_smart_contract.Contract.Service;
//using aspcore_async_deploy_smart_contract.AppService;
//using System.Collections.Generic;
//using System.Threading;
//using aspcore_async_deploy_smart_contract.Contract.DTO;
//using Nethereum.Web3;
//using aspcore_async_deploy_smart_contract.AppService.Services;

//namespace aspcore_bec.UnitTest
//{
//    public class TestBackgroundTxIdDeployService
//    {
//        private readonly ITestOutputHelper output;

//        public TestBackgroundTxIdDeployService(ITestOutputHelper output)
//        {
//            this.output = output;
//        }

//        [Theory]
//        [ClassData(typeof(HashTestData))]
//        public async void TestInsertRepoAsync(string[] hashs)
//        {
//            const string accountAddr = "";
//            const string password = "";
//            const string contractAddr = "";

//            var mockRepo = new Mock<IRepository<Certificate>>();
//            var mockLogger = new Mock<ILoggerService>();
//            var mockWeb3 = new Mock<Web3>();
//            var mockLoggerFactory = new Mock<ILoggerFactoryService>();
//            var mockTxIdQueue = new Mock<IBackgroundTaskQueue<(Guid id, Task<TransactionResult> task)>>();
//            var mockQuerryQueue = new Mock<IBackgroundTaskQueue<(Guid id, Task<ContractAddress> task)>>();
//            var mockScopeService = new Mock<IScopeService>();
//            var mockBECInterface = new Mock<IBECInterface>();

//            mockLogger.Setup(l => l.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()))
//                    .Callback((string s, object[] obj) => output.WriteLine($"inf: {s}", obj));
//            mockLogger.Setup(l => l.LogError(It.IsAny<string>()))
//                    .Callback((string s, object[] obj) => output.WriteLine($"err : {s}", obj));
//            mockLogger.Setup(l => l.LogDebug(It.IsAny<string>()))
//                    .Callback((string s, object[] obj) => output.WriteLine($"dbg : {s}", obj));
//            var loggerObj = mockLogger.Object;

//            mockLoggerFactory.Setup(lf => lf.CreateLogger<BackgroundContractDeploymentService>())
//                            .Returns(loggerObj);
//            var loggerFactoryObj = mockLoggerFactory.Object;

//            Queue<Func<CancellationToken, Task<(Guid id, Task<TransactionResult> task)>>> txidQueue = new Queue<Func<CancellationToken, Task<(Guid id, Task<TransactionResult> task)>>>();
//            mockTxIdQueue.Setup(q => q.QueueBackgroundWorkItem(It.IsAny<Func<CancellationToken, Task<(Guid id, Task<TransactionResult> task)>>>()))
//                        .Callback((Func<CancellationToken, Task<(Guid id, Task<TransactionResult> task)>> f) => txidQueue.Enqueue(f));
//            mockTxIdQueue.Setup(q => q.DequeueAsync(It.IsAny<CancellationToken>()))
//                        .ReturnsAsync(() => txidQueue.Dequeue());
//            mockTxIdQueue.Setup(q => q.Count)
//                        .Returns(() => txidQueue.Count);
//            var txidQueueObj = mockTxIdQueue.Object;

//            Queue<Func<CancellationToken, Task<(Guid id, Task<ContractAddress> task)>>> querryQueue = new Queue<Func<CancellationToken, Task<(Guid id, Task<ContractAddress> task)>>>();
//            mockQuerryQueue.Setup(q => q.QueueBackgroundWorkItem(It.IsAny<Func<CancellationToken, Task<(Guid id, Task<ContractAddress> task)>>>()))
//                        .Callback((Func<CancellationToken, Task<(Guid id, Task<ContractAddress> task)>> f) => querryQueue.Enqueue(f));
//            mockQuerryQueue.Setup(q => q.DequeueAsync(It.IsAny<CancellationToken>()))
//                        .ReturnsAsync(() => querryQueue.Dequeue());
//            mockQuerryQueue.Setup(q => q.Count)
//                        .Returns(() => querryQueue.Count);
//            var querryQueueObj = mockQuerryQueue.Object;

//            List<Certificate> certificates = new List<Certificate>();
//            mockRepo.Setup(rp => rp.Insert(It.IsAny<Certificate>()))
//                    .Callback((Certificate c) => certificates.Add(c));
//            mockRepo.Setup(rp => rp.GetById(It.IsAny<Guid>()))
//                    .Returns((Guid id) => certificates.Find(c => c.Id.Equals(id)));
//            mockRepo.Setup(rp => rp.Delete(It.IsAny<Certificate>()))
//                    .Callback((Certificate c) => certificates.RemoveAll(cc => cc.Id.Equals(c.Id)));
//            mockRepo.Setup(rp => rp.Update(It.IsAny<Certificate>()))
//                    .Callback((Certificate c) => {
//                        certificates.RemoveAll(cc => cc.Id.Equals(c.Id));
//                        certificates.Add(c);
//                    });
//            mockRepo.Setup(rp => rp.List())
//                    .Returns(() => certificates);
//            var repoObj = mockRepo.Object;

//            //mockScope.Setup(sc => sc.ServiceProvider.GetRequiredService<IRepository<Certificate>>())
//            //        .Returns(repoObj);
//            //var scopeObj = mockScope.Object;

//            //mockScopeFactory.Setup(sf => sf.CreateScope())
//            //                .Returns(scopeObj);
//            //var scopeFactoryObj = mockScopeFactory.Object;

//            mockScopeService.Setup(ss => ss.GetRequiredService<IRepository<Certificate>>())
//                            .Returns(repoObj);
//            var scopeObj = mockScopeService.Object;

//            mockBECInterface.Setup(b => b.DeployContract(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
//                            .Returns(async (string accAddr, string pw, string contrAddr, string hash) => {
//                                await Task.Delay(1000);
//                                return new TransactionResult(new Guid().ToString(), $"lala{hash}");
//                            });
//            mockBECInterface.Setup(b => b.QuerryReceipt(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), 1000))
//                            .Returns(async (string txid, int delay) => {
//                                await Task.Delay(delay);
//                                return new ContractAddress(new Guid().ToString(), $"lala{txid}");
//                            });
//            var becObj = mockBECInterface.Object;

//            var service = new BackgroundContractDeploymentService(txidQueueObj, querryQueueObj, loggerFactoryObj, scopeObj);
//            var taskDoneCount = 0;
//            var taskDoneCountShouldBe = hashs.Length;

//            foreach (var hash in hashs) {
//                var certEntity = new Certificate() {
//                    Id = Guid.NewGuid(),
//                    DeployStart = DateTime.UtcNow,
//                    DeployDone = default(DateTime),
//                    Hash = hash,
//                    Status = DeployStatus.Pending
//                };
//                loggerObj.LogInformation("Id: {0}, hash: {1}", certEntity.Id, certEntity.Hash);
//                certificates.Add(certEntity);

//                txidQueueObj.QueueBackgroundWorkItem((ct) => {
//                    return becObj.DeployContract(accountAddr, password, "", "", hash).ContinueWith(txid => {
//                        taskDoneCount++;
//                        return (certEntity.Id, txid);
//                    });
//                });
//            }

//            await service.StartAsync(CancellationToken.None);//.ConfigureAwait(false);

//            await Task.Delay(10000);

//            Assert.Equal(taskDoneCountShouldBe, taskDoneCount);

//            await service.StopAsync(CancellationToken.None);

//            Assert.True(true);
//            Assert.Equal(taskDoneCountShouldBe, querryQueue.Count);
//            foreach (var cert in certificates) {
//                output.WriteLine(cert.TransactionId);
//            }
//        }
//    }
//}
