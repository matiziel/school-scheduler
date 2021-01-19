using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Common;
using Persistence;
using Contracts.ViewModels.Dictionaries;
using Microsoft.EntityFrameworkCore;


namespace WebClient.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class DictionariesController : ControllerBase {
        private readonly IDictionariesService _disctionariesService;
        public DictionariesController(IDictionariesService disctionariesService) =>
            _disctionariesService = disctionariesService;
        private DataType GetValueFromString(string name) => (DataType)Enum.Parse(typeof(DataType), name);


    }
}