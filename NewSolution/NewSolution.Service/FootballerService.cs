using NewSolution.Model;
using NewSolution.Repository;
using NewSolution.Repository.Common;
using NewSolution.Service.Common;
using System;
using System.Collections.Generic;

namespace NewSolution.Service
{
    public class FootballerService : IFootballerService
    {
        public bool InsertFootballer(Footballer footballer)
        {
            FootballerRepository footballerRepository = new FootballerRepository();
            return footballerRepository.InsertFootballer(footballer);
        }

        public bool DeleteFootballerById(Guid id)
        {
            IFootballerRepository footballerRepository = new FootballerRepository();
            return footballerRepository.DeleteFootballerById(id);
        }

        public bool UpdateFootballerById(Guid id, Footballer footballer)
        {
            IFootballerRepository footballerRepository = new FootballerRepository();
            return footballerRepository.UpdateFootballerById(id, footballer);
        }

        public Footballer GetFootballerById(Guid id)
        {
            IFootballerRepository footballerRepository = new FootballerRepository();
            return footballerRepository.GetFootballerById(id);
        }

        public List<Footballer> GetFootballers()
        {
            IFootballerRepository footballerRepository = new FootballerRepository();
            return footballerRepository.GetFootballers();
        }
    }
}
