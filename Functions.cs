using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WordRankerAPIServerless.Controllers;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace WordRankerAPIServerless
{

    public class Functions
    {
        /// <summary>
        /// A Lambda function that returns result of word ranking
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Ranked Words, or Message to wait</returns>
        public async Task<APIGatewayProxyResponse> GetRankURL(APIGatewayProxyRequest request, ILambdaContext context)
        {
            string URL = request.Body;

            string result = WordRankerController.RankURL(URL);
            if (result.Length == 0) result = "Error Processing URL: " + URL;

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int) HttpStatusCode.OK,
                Body = JsonConvert.SerializeObject(result),
                Headers = new Dictionary<string, string> {{"Content-Type", "application/json"}}
            };

            return response;
        }
    }
}
