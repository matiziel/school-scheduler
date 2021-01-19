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
    public class CheckCorrectPaths {
        [Theory]
        [InlineData(DataType.ClassGroup, "1a")]
        [InlineData(DataType.Room, "111")]
        [InlineData(DataType.Subject, "eng")]
        [InlineData(DataType.Teacher, "kowalski")]
        public async Task GetDictionaryElementAsync(DataType type, string name) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new DictionariesService(context);
                var dictionaryElement = GetDictionaryElement(context, type, name);
                var dictionaryElementDTO = await service.GetDictionaryElementAsync(dictionaryElement.Item1, type);
                Assert.Equal(dictionaryElement.Item1, dictionaryElementDTO.Id);
                Assert.Equal(dictionaryElement.Item2, dictionaryElementDTO.Name);
            }
        }
        private ValueTuple<int, string> GetDictionaryElement(ApplicationDbContext context, DataType type, string name) {
            switch (type) {
                case DataType.ClassGroup:
                    var group = context.ClassGroups.FirstOrDefault(c => c.Name == name);
                    if(group is null)
                        return (-1, "");
                    return (group.Id, group.Name);
                case DataType.Room:
                    var room = context.Rooms.FirstOrDefault(c => c.Name == name);
                    if (room is null)
                        return (-1, "");
                    return (room.Id, room.Name);
                case DataType.Teacher:
                    var teacher = context.Teachers.FirstOrDefault(c => c.Name == name);
                    if (teacher is null)
                        return (-1, "");
                    return (teacher.Id, teacher.Name);
                case DataType.Subject:
                    var subject = context.Subjects.FirstOrDefault(c => c.Name == name);
                    if (subject is null)
                        return (-1, "");
                    return (subject.Id, subject.Name);
                default:
                    return (-1, "");
            }
        }
        [Theory]
        [InlineData(DataType.ClassGroup)]
        [InlineData(DataType.Room)]
        [InlineData(DataType.Subject)]
        [InlineData(DataType.Teacher)]
        public async Task GetDictionaryAsync(DataType type) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new DictionariesService(context);
                int size = GetSizeOfDictionary(context, type);
                var dictionaryIndexDTO = await service.GetDictionaryAsync(type);
                Assert.Equal(size, dictionaryIndexDTO.Count());
            }
        }
        public int GetSizeOfDictionary(ApplicationDbContext context, DataType type) {
            switch (type) {
                case DataType.ClassGroup:
                    return context.ClassGroups.Count();
                case DataType.Room:
                    return context.Rooms.Count();
                case DataType.Teacher:
                    return context.Teachers.Count();
                case DataType.Subject:
                    return context.Subjects.Count();
                default:
                    return 0;
            }
        }
        [Theory]
        [InlineData(DataType.ClassGroup)]
        [InlineData(DataType.Room)]
        [InlineData(DataType.Subject)]
        [InlineData(DataType.Teacher)]
        public async Task AddKeyTest(DataType type) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new DictionariesService(context);
                var dictionaryElement = new DictionaryElementCreateDTO() {
                    Name = "elo"
                };
                await service.AddKey(dictionaryElement, type);
                var value = GetDictionaryElement(context, type, dictionaryElement.Name);
                Assert.Equal(dictionaryElement.Name, value.Item2);
            }
        }
        [Theory]
        [InlineData(DataType.ClassGroup, "1a")]
        [InlineData(DataType.Room, "111")]
        [InlineData(DataType.Subject, "eng")]
        [InlineData(DataType.Teacher, "kowalski")]
        public async Task UpdateKey(DataType type, string name) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new DictionariesService(context);
                var element = GetDictionaryElement(context, type, name);
                var dictionaryElement = new DictionaryElementEditDTO() {
                    Id = element.Item1,
                    Name = "elo"
                };
                await service.UpdateKey(dictionaryElement, type);
                var value = GetDictionaryElement(context, type, "elo");
                Assert.Equal(dictionaryElement.Id, value.Item1);
            }
        }
        [Theory]
        [InlineData(DataType.ClassGroup, "1a")]
        [InlineData(DataType.Room, "111")]
        [InlineData(DataType.Subject, "eng")]
        [InlineData(DataType.Teacher, "kowalski")]
        public async Task RemoveKey(DataType type, string name) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new DictionariesService(context);
                var element = GetDictionaryElement(context, type, name);
                await service.RemoveKey(element.Item1, null, type);
                var value = GetDictionaryElement(context, type, "elo");
                Assert.Equal(-1, value.Item1);
            }
        }
        [Fact]
        public void GetFreeClassGroupsBySlot() {
            using (var context = PrepareData.GetDbContext()) {
                var service = new DictionariesService(context);
                var elements = service.GetFreeClassGroupsBySlot(0);
                int size = GetSizeOfDictionary(context, DataType.ClassGroup);
                Assert.Equal(size - 2, elements.Count());
            }
        }
        [Fact]
        public void GetFreeRoomsBySlot() {
            using (var context = PrepareData.GetDbContext()) {
                var service = new DictionariesService(context);
                var elements = service.GetFreeRoomsBySlot(0);
                int size = GetSizeOfDictionary(context, DataType.Room);
                Assert.Equal(size - 2, elements.Count());
            }
        }
        [Fact]
        public void GetFreeTeachersBySlot() {
            using (var context = PrepareData.GetDbContext()) {
                var service = new DictionariesService(context);
                var elements = service.GetFreeTeachersBySlot(0);
                int size = GetSizeOfDictionary(context, DataType.Teacher);
                Assert.Equal(size - 2, elements.Count());
            }
        }
    }
}