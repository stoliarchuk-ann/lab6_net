using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
    public class Exhibit : IComparable<Exhibit>
    {
        private string _name;
        private string _author;
        private int _count;

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Назва не може бути порожньою.", nameof(Name));
                }

                _name = value;
            }
        }

        public string Author
        {
            get
            {
                return _author;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Автор не може бути порожнім.", nameof(Author));
                }

                _author = value;
            }
        }

        public int Count
        {
            get
            {
                return _count;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Кількість заявок не може бути від'ємною.", nameof(Count));
                }

                _count = value;
            }
        }

        public Exhibit(string name, string author, int count)
        {
            Name = name;
            Author = author;
            Count = count;
        }

        public int CompareTo(Exhibit other)
        {
            if (other == null)
            {
                return 1;
            }
            return string.Compare(this.Name, other.Name, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return $"Назва: {Name}, автор: {Author}, кількість заявок: {Count}.";
        }
    }
}
