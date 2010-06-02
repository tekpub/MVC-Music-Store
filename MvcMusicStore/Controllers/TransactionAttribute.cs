using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers {
    public class TransactionAttribute:ActionFilterAttribute {
        public override void OnResultExecuted(ResultExecutedContext filterContext) {
            MvcApplication.Container.Get<IMusicRepo>().SaveChanges();
        }
    }
}