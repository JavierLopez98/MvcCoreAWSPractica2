using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAWSPractica2.Models
{
    [DynamoDBTable("Personas")]
    public class Persona
    {
        
        [DynamoDBProperty("IdUsuario")]
        [DynamoDBHashKey]
        public int IdUsuario { get; set; }
        [DynamoDBProperty("Nombre")]
        public String Nombre { get; set; }
        [DynamoDBProperty("Descripcion")]
        public String Descripcion { get; set; }
        [DynamoDBProperty("FechaAlta")]
        public String Fecha { get; set; }
        [DynamoDBProperty("Fotos")]
        public List<Foto> Fotos { get; set; }
    }
}
