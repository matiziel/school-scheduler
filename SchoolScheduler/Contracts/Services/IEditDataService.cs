using System.Collections.Generic;
using Contracts.ViewModels;
using Model;

namespace Contracts.Services {
    public interface IEditDataService {
        List<string> GetAllGroups();
        List<string> GetAllRooms();
        List<string> GetAllTeachers();
        List<string> GetAllClasses();
    }
}