﻿using Microsoft.AspNetCore.Authentication.OAuth;

namespace APIWithKeys.Middleware
{
    public class HandlingMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public HandlingMiddleware(RequestDelegate next,IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //ทำการดึง api key มาตรวจสอบถ้าไม่มีให้ return "API Key missing" 
            if (!context.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName,
                out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key missing");
                return;
            }
            //ทำการดึง api key มาตรวจสอบถ้า api key ไม่ถูกต้อง return "Invalid API Key" 
            var apikey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);
            if(!apikey.Equals(extractedApiKey))
            { 
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid API Key");
                return;
            }
            await _next(context);
        }
    }


}
