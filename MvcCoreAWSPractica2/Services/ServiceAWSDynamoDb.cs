using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using MvcCoreAWSPractica2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAWSPractica2.Services
{
    
    public class ServiceAWSDynamoDb
    {
        private DynamoDBContext context;
        private AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        public ServiceAWSDynamoDb()
        {
            
            this.context = new DynamoDBContext(client);
        }
        public async Task CreatePersonaAsync(Persona p)
        {
            await this.context.SaveAsync<Persona>(p);
        }
        public async Task<List<Persona>> GetPersonasAsync()
        {
            var tabla = this.context.GetTargetTable<Persona>();
            var scanOptions = new ScanOperationConfig();
            var results = tabla.Scan(scanOptions);
            List<Document> data = await results.GetNextSetAsync();
            IEnumerable<Persona> personas = this.context.FromDocuments<Persona>(data);
            return personas.ToList();
        }
        public async Task<Persona> BuscarPersonaAsync(int id)
        {
            return await this.context.LoadAsync<Persona>(id);
        }
        public async Task DeletePersonaAsync(int id)
        {
            await this.context.DeleteAsync<Persona>(id);
        }
        public async Task UpdatePersonaAsync(Persona p)
        {
            var tabla = this.context.GetTargetTable<Persona>();
            List<Foto> fotos = p.Fotos;
            Dictionary<string, AttributeValue> attributes = new Dictionary<string, AttributeValue>();
            attributes["IdUsuario"] = new AttributeValue { N = p.IdUsuario.ToString() };
            attributes["Nombre"] = new AttributeValue { S = p.Nombre.ToString() };
            attributes["Descripcion"] = new AttributeValue { S = p.Descripcion.ToString() };
            attributes["FechaAlta"] = new AttributeValue { S = p.Fecha.ToString() };
            if (fotos != null)
            {
                attributes["Fotos"] = new AttributeValue {    };
            }
            PutItemRequest request = new PutItemRequest
            {
                TableName = "Personas",
                Item = attributes
            };
            await client.PutItemAsync(request);
        }
    }
}
