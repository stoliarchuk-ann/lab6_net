using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
    public class ExhibitCatalog
    {
        private readonly IFileSaver _fileSaver;
        private readonly IFileLoader _fileLoader;
        private readonly string _filePath;
        public ObservableCollection<Exhibit> Exhibits { get; }
        private List<Exhibit> _allExhibits;

        public ExhibitCatalog(IFileSaver fileSaver, IFileLoader fileLoader, string filePath)
        {
            _fileSaver = fileSaver;
            _fileLoader = fileLoader;
            _filePath = filePath;
            _allExhibits = new List<Exhibit>();
            Exhibits = new ObservableCollection<Exhibit>();
        }

        public void AddExhibit(Exhibit exhibit)
        {
            Exhibits.Add(exhibit);
            _allExhibits.Add(exhibit);
        }

        public void RemoveExhibit(Exhibit exhibit)
        {
            Exhibits.Remove(exhibit);
            _allExhibits.Remove(exhibit);
        }

        public void ResetFilter()
        {
            UpdateVisibleCollection(_allExhibits);
        }

        public void SortByCount()
        {
            var sortedList = _allExhibits.OrderBy(e => e.Count).ThenBy(e => e.Name).ToList();
            UpdateVisibleCollection(sortedList);
        }

        public void SearchExhibits(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                ResetFilter();
                return;
            }
            string lowerTerm = searchTerm.ToLowerInvariant();
            var searchResult = _allExhibits.Where(e =>
                e.Name.ToLowerInvariant().Contains(lowerTerm) ||
                e.Author.ToLowerInvariant().Contains(lowerTerm)
            ).ToList();

            UpdateVisibleCollection(searchResult);
        }

        public void SearchUnikAuthors()
        {
            var uniqueAuthors = _allExhibits
                .Select(e => e.Author)
                .Distinct()
                .OrderBy(a => a)
                .ToList();

            var authorExhibits = uniqueAuthors.Select(a => new Exhibit("Категорія", a, 0) { Author = a }).ToList();
            UpdateVisibleCollection(authorExhibits);
        }

        public void FindTop3PopularExhibits()
        {
            var topExhibits = _allExhibits
                .OrderByDescending(e => e.Count)
                .Take(3)
                .ToList();

            UpdateVisibleCollection(topExhibits);
        }

        public double CalculateAverageCount()
        {
            if (_allExhibits.Count == 0)
            {
                return 0;
            }
            return _allExhibits.Average(e => e.Count);
        }

        public void GroupExhibitsByAuthor()
        {
            var grouped = _allExhibits
                .GroupBy(e => e.Author)
                .OrderBy(g => g.Key)
                .SelectMany(g => g)
                .ToList();

            UpdateVisibleCollection(grouped);
        }

        private void UpdateVisibleCollection(List<Exhibit> newList)
        {
            Exhibits.Clear();
            foreach (var exhibit in newList)
            {
                Exhibits.Add(exhibit);
            }
        }

        public async Task SaveAsync()
        {
            await _fileSaver.SaveAsync(_filePath, _allExhibits);
        }

        public async Task LoadAsync()
        {
            var loadedExhibits = await _fileLoader.LoadAsync<Exhibit>(_filePath);
            if (loadedExhibits != null)
            {
                _allExhibits = loadedExhibits;
                UpdateVisibleCollection(_allExhibits);
            }
        }
    }
}
