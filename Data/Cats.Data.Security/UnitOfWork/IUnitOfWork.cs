﻿using System;
using System.Collections.Generic;
using Cats.Models.Security;

namespace Cats.Data.Security
{
    /// <summary>
    /// UnitOfWork class to manage persistence and retrival of modles associated with
    /// the security module
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<UserInfo> UserInfoRepository { get; }
        IGenericRepository<UserProfile> UserProfileRepository { get; }
        IGenericRepository<ForgetPasswordRequest> ForgetPasswordRequestRepository { get; }
        IGenericRepository<Setting> SettingRepository { get; } 
        void Save();
    }
}
