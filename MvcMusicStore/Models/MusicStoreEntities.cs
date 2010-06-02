using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using System.Data.Objects;

namespace MvcMusicStore.Models {
    public partial class MusicStoreEntities {
        public static MusicStoreEntities Current {
            get {
                return (MusicStoreEntities)MvcApplication.Container.Get<ObjectContext>();
            }
        }
    }
}