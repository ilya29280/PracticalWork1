using System.Collections.Generic;
using System.Linq;
using PW1.Model;
using PW1.DataAccessLayer;

namespace PW1.BusinessLogic
{
    public class Logic
    {
        private readonly IRepository<Gnome> _repository;

        // Репозиторий передается снаружи (внедрение зависимости)
        public Logic(IRepository<Gnome> repository)
        {
            _repository = repository;
        }

        public void AddGnome(string name, string clan, string profession, int age, int beardLength)
        {
            var gnome = new Gnome { Name = name, Clan = clan, Profession = profession, Age = age, BeardLength = beardLength };
            _repository.Add(gnome);
        }

        public List<Gnome> GetAllGnomes()
        {
            return _repository.ReadAll();
        }

        public bool UpdateGnome(int id, string newName, string newClan, string newProfession, int newAge, int newBeardLength)
        {
            var gnome = _repository.ReadById(id);
            if (gnome == null) return false;

            gnome.Name = newName; gnome.Clan = newClan; gnome.Profession = newProfession;
            gnome.Age = newAge; gnome.BeardLength = newBeardLength;

            _repository.Update(gnome);
            return true;
        }

        public bool DeleteGnome(int id)
        {
            if (_repository.ReadById(id) == null) return false;
            _repository.Delete(id);
            return true;
        }

        public Dictionary<string, List<Gnome>> GroupGnomesByClan()
        {
            return GetAllGnomes().GroupBy(g => g.Clan).ToDictionary(g => g.Key, g => g.ToList());
        }

        public List<Gnome> GetGnomesByProfession(string profession)
        {
            return GetAllGnomes()
                .Where(g => g.Profession.ToLower().Contains(profession.ToLower()))
                .OrderBy(g => g.Age).ToList();
        }
    }
}