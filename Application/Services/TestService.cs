using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using DataAccess.Interfaces;
using Models.Entities;

namespace Application.Services
{
    public class TestService : ITestService
    {
        //private IGeneric<Test> TestRepo { set; get; }

        /*public TestService(IGeneric<Test> testRepo)
        {
            this.TestRepo = testRepo;
        }*/

        public async Task RunTest(CancellationToken token)
        {
            /*await TestRepo.InsertAsync(new Test()
            {
                Name = "zzzzzzz"
            }, token);*/

            /*var r = await TestRepo.GetAsync(30, token);
            Console.WriteLine(r==null);*/

            /*
            var res1 = await TestRepo.ListAsync(token);
            foreach (var v in res1)
            {
                Console.WriteLine($"{v.Id} {v.Name}");
            }

            var t = await TestRepo.GetAsync(1, token);
            */

            //t.Name = "AAAAAA";

            //await TestRepo.UpdateAsync(t, token);
            //await TestRepo.DeleteAsync(t, token);

            /*var res2 = await TestRepo.ListAsync(token);
            foreach (var v in res2)
            {
                Console.WriteLine($"{v.Id} {v.Name}");
            }*/


            /*var res2 = await TestRepo.ListAsync(test => test.Id == 1, token);
            foreach (var v in res2)
            {
                Console.WriteLine($"{v.Id} {v.Name}");
            }*/


            //
            //
            //
        }
    }
}