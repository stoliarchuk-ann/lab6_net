using lab6;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1
{
    [TestClass]
    public class ExhibitTests
    {
        [TestMethod]
        public void Constructor_ValidData_SetsPropertiesCorrectly()
        {
            string name = "Ваза";
            string author = "Невідомий";
            int count_request = 15;

            Exhibit exhibit = new Exhibit(name, author, count_request);
            Assert.AreEqual(name, exhibit.Name);
            Assert.AreEqual(author, exhibit.Author);
            Assert.AreEqual(count_request, exhibit.Count);
        }

        [TestMethod]
        public void Count_SetNegativeValue_ThrowsArgumentException()
        {
            string name = "Ваза";
            string author = "Невідомий";
            int count_request = -5;

            Assert.ThrowsException<ArgumentException>(() => new Exhibit(name, author, count_request));
        }

        [TestMethod]
        public void Name_SetEmptyValue_ThrowsArgumentException()
        {
            string name = "";
            string author = "Невідомий";
            int count_request = 15;

            Assert.ThrowsException<ArgumentException>(() => new Exhibit(name, author, count_request));
        }

        [TestMethod]
        public void CompareTo_ComparesByName_ReturnsCorrectOrder()
        {
            string name1 = "А-експонат";
            string author1 = "Невідомий";
            int count_request1 = 15;
            string name2 = "Б-експонат";
            string author2 = "Невідомий";
            int count_request2 = 15;

            Exhibit exhibit1 = new Exhibit(name1, author1, count_request1);
            Exhibit exhibit2 = new Exhibit(name2, author2, count_request2);

            int result = exhibit1.CompareTo(exhibit2);
            Assert.IsTrue(result < 0);
        }

        [TestMethod]
        public void Count_SetZeroValue_SetsPropertlyCorrectly()
        {
            string name = "Ваза";
            string author = "Невідомий";
            int count_request = 0;

            Exhibit exhibit = new Exhibit(name, author, count_request);
            Assert.AreEqual(0, exhibit.Count);
        }

        [TestMethod]
        public void Author_SetEmptyValue_ThrowsArgumentException()
        {
            string name = "Ваза";
            string author = "   ";
            int count_request = 15;

            Assert.ThrowsException<ArgumentException>(() => new Exhibit(name, author, count_request));
        }

        [TestMethod]
        public void ToString_ReturnsFormattedString()
        {
            string name = "Ваза";
            string author = "Невідомий";
            int count_request = 15;

            Exhibit exhibit = new Exhibit(name, author, count_request);
            Assert.AreEqual("Назва: Ваза, автор: Невідомий, кількість заявок: 15.", exhibit.ToString());
        }

        [TestMethod]
        public void CompareTo_NullObject_ReturnsOne()
        {
            string name = "Ваза";
            string author = "Невідомий";
            int count_request = 15;

            Exhibit exhibit = new Exhibit(name, author, count_request);
            int result = exhibit.CompareTo(null);
            Assert.AreEqual(1, result);
        }
    }

    [TestClass]
    public class ExhibitCatalogTests
    {
        private ExhibitCatalog _catalog;
        private Exhibit _exhibit1;
        private Exhibit _exhibit2;
        private Exhibit _exhibit3;
        private Exhibit _exhibit4;

        [TestInitialize]
        public void TestInitialize()
        {
            _exhibit1 = new Exhibit("Картина", "Шевченко", 5);
            _exhibit2 = new Exhibit("Скульптура", "Пінзель", 1);
            _exhibit3 = new Exhibit("Ваза", "Невідомий", 9);
            _exhibit4 = new Exhibit("Ескіз", "Шевченко", 3);

            _catalog = new ExhibitCatalog(null, null, "dummy.json");
            _catalog.AddExhibit(_exhibit1);
            _catalog.AddExhibit(_exhibit2);
            _catalog.AddExhibit(_exhibit3);
            _catalog.AddExhibit(_exhibit4);
        }

        [TestMethod]
        public void AddExhibit_AddsItemToList()
        {
            ExhibitCatalog catalog = new ExhibitCatalog(new JsonFileWriter(), new JsonFileReader(), "test.json");

            string name = "Ваза";
            string author = "Невідомий";
            int count_request = 15;

            Exhibit exhibit = new Exhibit(name, author, count_request);

            catalog.AddExhibit(exhibit);
            Assert.AreEqual(1, catalog.Exhibits.Count);
        }

        [TestMethod]
        public void RemoveExhibit_RemovesItemFromList()
        {
            ExhibitCatalog catalog = new ExhibitCatalog(new JsonFileWriter(), new JsonFileReader(), "test.json");

            string name = "Ваза";
            string author = "Невідомий";
            int count_request = 15;

            Exhibit exhibit = new Exhibit(name, author, count_request);

            catalog.AddExhibit(exhibit);
            catalog.RemoveExhibit(exhibit);

            Assert.AreEqual(0, catalog.Exhibits.Count);
        }

        [TestMethod]
        public void RemoveExhibit_ItemNotInList_DoesNothing()
        {
            ExhibitCatalog catalog = new ExhibitCatalog(new JsonFileWriter(), new JsonFileReader(), "test.json");

            string name = "Ваза";
            string author = "Невідомий";
            int count_request = 15;

            Exhibit exhibit = new Exhibit(name, author, count_request);
            Exhibit exhibit1 = new Exhibit(name, author, count_request);

            catalog.AddExhibit(exhibit);
            catalog.RemoveExhibit(exhibit1);

            Assert.AreEqual(1, catalog.Exhibits.Count);
        }

        [TestMethod]
        public void CalculateAverageCount_ReturnsCorrectValue()
        {
            double result = _catalog.CalculateAverageCount();
            Assert.AreEqual(4.5, result);
        }

        [TestMethod]
        public void SearchExhibits_WithValidFragment()
        {
            _catalog.SearchExhibits("евч");
            Assert.AreEqual(2, _catalog.Exhibits.Count);
        }

        [TestMethod]
        public void SortByCount_ReturnsSortedByAscending()
        {
            _catalog.SortByCount();
            Assert.AreEqual(1, _catalog.Exhibits[0].Count);
            Assert.AreEqual(3, _catalog.Exhibits[1].Count);
            Assert.AreEqual(5, _catalog.Exhibits[2].Count);
            Assert.AreEqual(9, _catalog.Exhibits[3].Count);
        }

        [TestMethod]
        public void FindTop3PopularExhibits_ReturnsTopThree()
        {
            _catalog.FindTop3PopularExhibits();
            Assert.AreEqual(3, _catalog.Exhibits.Count);
            Assert.AreEqual(9, _catalog.Exhibits[0].Count);
            Assert.AreEqual(5, _catalog.Exhibits[1].Count);
            Assert.AreEqual(3, _catalog.Exhibits[2].Count);
        }

        [TestMethod]
        public void SearchUnikAuthors_ReturnsDistinctAuthors()
        {
            _catalog.SearchUnikAuthors();
            Assert.AreEqual(3, _catalog.Exhibits.Count);
            Assert.AreEqual("Невідомий", _catalog.Exhibits[0].Author);
            Assert.AreEqual("Пінзель", _catalog.Exhibits[1].Author);
            Assert.AreEqual("Шевченко", _catalog.Exhibits[2].Author);
        }

        [TestMethod]
        public void GroupExhibitsByAuthor_GroupsAndSortsAlphabetically()
        {
            _catalog.GroupExhibitsByAuthor();
            Assert.AreEqual("Невідомий", _catalog.Exhibits[0].Author);
            Assert.AreEqual("Пінзель", _catalog.Exhibits[1].Author);
            Assert.AreEqual("Шевченко", _catalog.Exhibits[2].Author);
            Assert.AreEqual("Шевченко", _catalog.Exhibits[3].Author);
        }
    }

    [TestClass]
    public class FileHandlerTests
    {
        private string _testFilePath;

        [TestInitialize]
        public void TestInitialize()
        {
            _testFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".json");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [TestMethod]
        public async Task SaveAsync_Should_WriteDataToFile()
        {
            List<Exhibit> list = new List<Exhibit>()
            {
                new Exhibit("Ваза", "Невідомий", 15)
            };

            var saver = new JsonFileWriter();
            await saver.SaveAsync(_testFilePath, list);

            Assert.IsTrue(File.Exists(_testFilePath));

            string fileContent = await File.ReadAllTextAsync(_testFilePath);
            Assert.IsFalse(string.IsNullOrWhiteSpace(fileContent));

            var reader = new JsonFileReader();
            var loadedList = await reader.LoadAsync<Exhibit>(_testFilePath);

            Assert.IsNotNull(loadedList);
            Assert.AreEqual(1, loadedList.Count);
        }

        [TestMethod]
        public async Task LoadAsync_Should_ReadDataFromFile()
        {
            string jsonContent = "[{\"Name\":\"Ваза\",\"Author\":\"Невідомий\",\"Count\":15}]";
            await File.WriteAllTextAsync(_testFilePath, jsonContent);

            var reader = new JsonFileReader();
            var loadedList = await reader.LoadAsync<Exhibit>(_testFilePath);

            Assert.IsNotNull(loadedList);
            Assert.AreEqual(1, loadedList.Count);
            Assert.AreEqual("Ваза", loadedList[0].Name);
        }

        [TestMethod]
        public async Task LoadAsync_WhenFileDoesNotExist_ReturnsNull()
        {
            var reader = new JsonFileReader();
            var loadedList = await reader.LoadAsync<Exhibit>(_testFilePath);

            Assert.IsNull(loadedList);
        }
    }
}