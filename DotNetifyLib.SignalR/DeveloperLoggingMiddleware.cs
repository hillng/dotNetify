﻿/* 
Copyright 2017 Dicky Suryadi

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
 */

using System;
using System.Threading.Tasks;
using DotNetify;
using Microsoft.AspNetCore.SignalR.Hubs;
using Newtonsoft.Json;

namespace WebApplication.Core.React
{
   public delegate void LogTraceDelegate(string log);

   public class DeveloperLoggingMiddleware : IMiddleware, IDisconnectionMiddleware, IExceptionMiddleware
   {
      private readonly LogTraceDelegate _trace;

      public DeveloperLoggingMiddleware(LogTraceDelegate trace)
      {
         _trace = trace;
      }

      public Task Invoke(DotNetifyHubContext hubContext, NextDelegate next)
      {
         _trace($@"[dotNetify] connId={hubContext.CallerContext.ConnectionId} type={hubContext.CallType} 
            vmId={hubContext.VMId} 
            data={JsonConvert.SerializeObject(hubContext.Data ?? string.Empty)} 
            headers={JsonConvert.SerializeObject(hubContext.Headers ?? string.Empty)}");

         return next(hubContext);
      }

      public Task OnDisconnected(HubCallerContext context)
      {
         _trace($"[dotNetify] connId={context.ConnectionId} type=OnDisconnected");
         return Task.CompletedTask;
      }

      public Task<Exception> OnException(HubCallerContext context, Exception exception)
      {
         _trace($"[dotNetify] connId={context.ConnectionId} {exception.GetType().Name}={exception.Message}");
         return Task.FromResult(exception);
      }
   }

   /// <summary>
   /// Method extension to specify parameter type for the middleware.
   /// </summary>
   public static class DeveloperLoggingMiddlewareExtensions
   {
      public static void UseDeveloperLogging(this IDotNetifyConfiguration config, LogTraceDelegate logTraceDelegate)
      {
         config.UseMiddleware<DeveloperLoggingMiddleware>(logTraceDelegate);
      }
   }
}