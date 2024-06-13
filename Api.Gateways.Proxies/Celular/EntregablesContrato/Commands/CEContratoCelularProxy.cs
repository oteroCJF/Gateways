﻿using System.Net.Http;
using System.Net.Http.Headers;
using Api.Gateway.Proxies.Config;
using Api.Gateways.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Contratos;
using Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Contratos;
using System;

namespace Api.Gateway.Proxies.Celular.EntregablesContrato.Commands
{
    public interface ICEContratoCelularProxy
    {
        Task<int> UpdateEntregable([FromForm] EntregableContratoUpdateCommand command);
    }
    public class CEContratoCelularProxy : ICEContratoCelularProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        public CEContratoCelularProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromMinutes(10);
            _apiUrls = apiUrls.Value;
        }
        
        public async Task<int> UpdateEntregable([FromForm] EntregableContratoUpdateCommand entregable)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(entregable.Id.ToString()), "Id");
            formContent.Add(new StringContent(entregable.UsuarioId.ToString()), "UsuarioId");
            formContent.Add(new StringContent(entregable.EntregableId.ToString()), "EntregableId");
            formContent.Add(new StringContent(entregable.FechaProgramada.ToString()), "FechaProgramada");
            formContent.Add(new StringContent(entregable.FechaEntrega.ToString()), "FechaEntrega");
            formContent.Add(new StringContent(entregable.InicioVigencia.ToString()), "InicioVigencia");
            formContent.Add(new StringContent(entregable.FinVigencia.ToString()), "FinVigencia");
            formContent.Add(new StringContent(entregable.MontoGarantia.ToString()), "MontoGarantia");
            formContent.Add(new StringContent(entregable.Penalizable.ToString()), "Penalizable");
            formContent.Add(new StringContent(entregable.MontoPenalizacion.ToString()), "MontoPenalizacion");
            formContent.Add(new StringContent(entregable.Observaciones.ToString()), "Observaciones");

            if (entregable.Archivo != null)
            {
                formContent.Add(new StringContent(entregable.Contrato.ToString()), "Contrato");
                formContent.Add(new StringContent(entregable.Convenio.ToString()), "Convenio");
                formContent.Add(new StringContent(entregable.TipoEntregable.ToString()), "TipoEntregable");
                var fileStreamContentPDF = new StreamContent(entregable.Archivo.OpenReadStream());
                fileStreamContentPDF.Headers.ContentType = MediaTypeHeaderValue.Parse(entregable.Archivo.ContentType);
                formContent.Add(fileStreamContentPDF, name: "Archivo", entregable.Archivo.FileName);
            }

            var request = await _httpClient.PutAsync($"{_apiUrls.CelularUrl}api/celular/entregablesContratacion/updateEntregableContratacion", formContent);
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<int>(
               await request.Content.ReadAsStringAsync(),
               new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true
               }
           );
        }

    }
}
