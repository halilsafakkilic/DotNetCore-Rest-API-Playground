﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using netCorePlayground.DTOs;
using netCorePlayground.Infra;
using netCorePlayground.Infra.Extensions;

namespace netCorePlayground.Controllers.User
{
    public class WeatherForecastController : UserController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger) : base()
        {
            _logger = logger;
        }

        /// <summary>
        /// Must be required Authorization to read
        /// </summary>
        [HttpGet]
        [Authorize(Policy = Policies.UserPolicy)]
        [Route("v1/list")]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogDebug($"User route called.");

            if (!User.CanView())
            {
                throw new UnauthorizedAccessException();
            }

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

        }

        [HttpGet]
        [Obsolete("Temporary time was added for trial")]
        [Authorize(Policy = Policies.UserPolicy)]
        [Route("v1.1-alpha/list")]
        public IActionResult GetV11Alpha()
        {
            return NoContent();
        }

        /// <summary>
        /// For much closer estimates
        /// </summary>
        /// <remarks>It only presents data from the last 24 hours.</remarks>
        [HttpGet]
        [Authorize(Policy = Policies.UserPolicy)]
        [Route("v1.2/list")]
        public IEnumerable<WeatherForecast> GetV12()
        {
            return Get();
        }
    }
}
