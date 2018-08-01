﻿using SMSApp.DataAccess;
using System.Collections;
using System.Collections.Generic;

namespace SMSApp.Repositories.Core
{
    public interface IPhotoRepository : IRepository<Photo>
    {
        void AddAndStore(Photo photo);
    }
}
