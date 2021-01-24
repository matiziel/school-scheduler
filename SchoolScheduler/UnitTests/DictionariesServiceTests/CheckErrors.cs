using Xunit;
using System.Threading.Tasks;
using Application;
using System;
using Common;
using Persistence;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Contracts.DataTransferObjects.Dictionaries;

namespace UnitTests.DictionariesServiceTests {
    // public class CheckErrors {
    //     [Theory]
    //     [InlineData(DataType.classGroups)]
    //     [InlineData(DataType.rooms)]
    //     [InlineData(DataType.subjects)]
    //     [InlineData(DataType.teachers)]
    //     public async Task GetDictionaryElementAsync(DataType type) {
    //         using (var context = PrepareData.GetDbContext()) {
    //             var service = new DictionariesService(context);
    //             await Assert.ThrowsAsync<ArgumentException>(
    //                 async () => await service.GetDictionaryElementAsync(2137, type));
    //         }
    //     }
    //     private ValueTuple<int, string> GetDictionaryElement(ApplicationDbContext context, DataType type, string name) {
    //         switch (type) {
    //             case DataType.classGroups:
    //                 var group = context.ClassGroups.FirstOrDefault(c => c.Name == name);
    //                 if (group is null)
    //                     return (-1, "");
    //                 return (group.Id, group.Name);
    //             case DataType.rooms:
    //                 var room = context.Rooms.FirstOrDefault(c => c.Name == name);
    //                 if (room is null)
    //                     return (-1, "");
    //                 return (room.Id, room.Name);
    //             case DataType.teachers:
    //                 var teacher = context.Teachers.FirstOrDefault(c => c.Name == name);
    //                 if (teacher is null)
    //                     return (-1, "");
    //                 return (teacher.Id, teacher.Name);
    //             case DataType.subjects:
    //                 var subject = context.Subjects.FirstOrDefault(c => c.Name == name);
    //                 if (subject is null)
    //                     return (-1, "");
    //                 return (subject.Id, subject.Name);
    //             default:
    //                 return (-1, "");
    //         }
    //     }
    //     [Theory]
    //     [InlineData(DataType.classGroups, "1a")]
    //     [InlineData(DataType.rooms, "111")]
    //     [InlineData(DataType.subjects, "eng")]
    //     [InlineData(DataType.teachers, "kowalski")]
    //     public async Task AddKey_CreateElementWhichNameAlreadyExists_ThrowsInvalidOperationException(
    //         DataType type, string name) {
    //         using (var context = PrepareData.GetDbContext()) {
    //             var service = new DictionariesService(context);
    //             var dictionaryElement = new DictionaryElementCreateDTO() {
    //                 Name = name
    //             };
    //             await Assert.ThrowsAsync<InvalidOperationException>(
    //                 async () => await service.AddKey(dictionaryElement, type));
    //         }
    //     }
    //     [Theory]
    //     [InlineData(DataType.classGroups)]
    //     [InlineData(DataType.rooms)]
    //     [InlineData(DataType.subjects)]
    //     [InlineData(DataType.teachers)]
    //     public async Task UpdateKey_EditElementWhichNotExists_ThrowsArgumentException(DataType type) {
    //         using (var context = PrepareData.GetDbContext()) {
    //             var service = new DictionariesService(context);
    //             var dictionaryElement = new DictionaryElementEditDTO() {
    //                 Id = 1000
    //             };
    //             await Assert.ThrowsAsync<ArgumentException>(
    //                 async () => await service.UpdateKey(dictionaryElement, type));
    //         }
    //     }
    //     [Theory]
    //     [InlineData(DataType.classGroups, "1a", "2a")]
    //     [InlineData(DataType.rooms, "111", "112")]
    //     [InlineData(DataType.subjects, "eng", "mat")]
    //     [InlineData(DataType.teachers, "kowalski", "nowak")]
    //     public async Task UpdateKey_EditElementWhichNameAlreadyExists_ThrowsInvalidOperationException(
    //         DataType type, string name, string newName) {
    //         using (var context = PrepareData.GetDbContext()) {
    //             var service = new DictionariesService(context);
    //             var element = GetDictionaryElement(context, type, name);
    //             var dictionaryElement = new DictionaryElementEditDTO() {
    //                 Id = element.Item1,
    //                 Name = newName
    //             };
    //             await Assert.ThrowsAsync<InvalidOperationException>(
    //                 async () => await service.UpdateKey(dictionaryElement, type));
    //         }
    //     }
    //     [Theory]
    //     [InlineData(DataType.classGroups)]
    //     [InlineData(DataType.rooms)]
    //     [InlineData(DataType.subjects)]
    //     [InlineData(DataType.teachers)]
    //     public async Task UpdateKey_DeleteElementWhichNotExists_ThrowsArgumentException(DataType type) {
    //         using (var context = PrepareData.GetDbContext()) {
    //             var service = new DictionariesService(context);
    //             await Assert.ThrowsAsync<InvalidOperationException>(
    //                 async () => await service.RemoveKey(10000, null, type));
    //         }
    //     }
    // }
}