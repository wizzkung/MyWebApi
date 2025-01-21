namespace MyWebApi.Model
{
    public class Books
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
    }


    public class AuthorMain
    {
        public string Author { get; set; }
        public Books[] Books { get; set; }
    }

    public class Book2
    {
        public string Name { get; set; }
    }

}
