using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcCoreAWSPractica2.Models;
using MvcCoreAWSPractica2.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAWSPractica2.Controllers
{
    public class PersonasController : Controller
    {
        private ServiceAWSDynamoDb ServiceDynamo;
        private ServiceAWSS3Files ServiceS3;
        public PersonasController (ServiceAWSS3Files S3,ServiceAWSDynamoDb Dynamo)
        {
            this.ServiceDynamo = Dynamo;
            this.ServiceS3 = S3;
        }
        public async Task<IActionResult> Index()
        {
            return View(await this.ServiceDynamo.GetPersonasAsync()) ;
        }
        public async Task<IActionResult> Details(int id)
        {
            return View(await this.ServiceDynamo.BuscarPersonaAsync(id));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Persona persona, String incluirfoto,String titulo1,
            IFormFile foto1,String titulo2,IFormFile foto2)
        {
            if (incluirfoto != null)
            {
                persona.Fotos = new List<Foto>();
                if (titulo1 != null)
                {
                    Foto f1 = new Foto();
                    f1.Titulo = titulo1;
                    await this.ServiceS3.UploadFile(foto1,persona.Nombre);
                    f1.Imagen = "https://fotos-practica.s3.amazonaws.com/" + persona.Nombre + foto1.FileName;
                    persona.Fotos.Add(f1);
                }
                if (titulo2 != null)
                {
                    Foto f2 = new Foto();
                    f2.Titulo = titulo1;
                    await this.ServiceS3.UploadFile(foto2, persona.Nombre);
                    f2.Imagen = "https://fotos-practica.s3.amazonaws.com/" + persona.Nombre + foto2.FileName;
                    persona.Fotos.Add(f2);
                }
            }
            await this.ServiceDynamo.CreatePersonaAsync(persona);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {

            return View(await this.ServiceDynamo.BuscarPersonaAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Persona persona)
        {
            Persona p = await this.ServiceDynamo.BuscarPersonaAsync(persona.IdUsuario);
            p.Nombre = persona.Nombre;
            p.Descripcion = persona.Descripcion;
            p.Fecha = persona.Fecha;
            await this.ServiceDynamo.UpdatePersonaAsync(p);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            Persona persona = await this.ServiceDynamo.BuscarPersonaAsync(id);
            if (persona.Fotos != null)
            {
                foreach (Foto f in persona.Fotos)
                {
                    String cadena = f.Imagen.Substring(f.Imagen.LastIndexOf('/')+1);
                    await this.ServiceS3.DeleteFile(cadena);
                }
            }
            await this.ServiceDynamo.DeletePersonaAsync(id);
            return RedirectToAction("Index");
        }
        
    }
}
