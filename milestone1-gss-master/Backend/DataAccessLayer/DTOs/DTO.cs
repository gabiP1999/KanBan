﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.DataAccessLayer.DTOs
{
     public abstract class DTO
    {
        public const string IDColumnName = "ID";
        protected DalController _controller;
        public long Id { get; set; } = -1;
        protected DTO(DalController controller)
        {
            _controller = controller;
        }

    }
}