﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Cats.Data.UnitWork;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
    public interface IReliefRequisitionService
    {
       
        bool AddReliefRequisition(ReliefRequisition reliefRequisition);
        bool DeleteReliefRequisition(ReliefRequisition reliefRequisition);
        bool DeleteById(int id);
        bool EditReliefRequisition(ReliefRequisition reliefRequisition);
        ReliefRequisition FindById(int id);
        List<ReliefRequisition> GetAllReliefRequisition();
        List<ReliefRequisition> FindBy(Expression<Func<ReliefRequisition, bool>> predicate);
        void AddReliefRequisions(List<ReliefRequisition> reliefRequisitions);

        List<ReliefRequisition> GetApprovedRequistion();

        //IEnumerable<ReliefRequisition> Get(
        //   Expression<Func<ReliefRequisition, bool>> filter = null,
        //   Func<IQueryable<ReliefRequisition>, IOrderedQueryable<ReliefRequisition>> orderBy = null,
        //   string includeProperties = "");


        IEnumerable<RegionalRequest> Get(
          Expression<Func<RegionalRequest, bool>> filter = null,
          Func<IQueryable<RegionalRequest>, IOrderedQueryable<RegionalRequest>> orderBy = null,
          string includeProperties = "");


        bool Save();
      


    }
}

