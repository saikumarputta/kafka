using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Api.Repository;
using Api.Services;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderRequestRepository _orderRequestRepository;
        private readonly ProducerConfig config;
        public OrderController(ProducerConfig config, IOptions<ConnectionStringList> connectionString)
        {
            this.config = config;
            _orderRequestRepository = new OrderRequestRepository(connectionString);

        }
        // POST api/values
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody]OrderRequest value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Serialize 
            string serializedOrder = JsonConvert.SerializeObject(value);

            Console.WriteLine("========");
            Console.WriteLine("Info: OrderController => Post => Recieved a new purchase order:");
            Console.WriteLine(serializedOrder);
            Console.WriteLine("=========");

            var producer = new ProducerWrapper(this.config,"orderrequests");
            await producer.writeMessage(serializedOrder);

            //Deserialize
            var value1 = JsonConvert.DeserializeObject<OrderRequest>(serializedOrder);
            _orderRequestRepository.Create(value1);
            return Created("TransactionId", "Your order is in progress");
        }
    }
}
