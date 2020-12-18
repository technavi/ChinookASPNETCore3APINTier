using System.Collections.Generic;
using Chinook.Domain.Entities;


namespace Chinook.Domain.Supervisor
{
    public interface IChinookSupervisor
    {
        IEnumerable<Album> GetAllAlbum();
        Album GetAlbumById(int id);
        IEnumerable<Album> GetAlbumByArtistId(int id);

        Album AddAlbum(Album newAlbum);

        bool UpdateAlbum(Album album);
        bool DeleteAlbum(int id);
        IEnumerable<Artist> GetAllArtist();
        Artist GetArtistById(int id);

        Artist AddArtist(Artist newArtist);

        bool UpdateArtist(Artist artist);

        bool DeleteArtist(int id);
        IEnumerable<Customer> GetAllCustomer();
        Customer GetCustomerById(int id);

        IEnumerable<Customer> GetCustomerBySupportRepId(int id);

        Customer AddCustomer(Customer newCustomer);

        bool UpdateCustomer(Customer customer);

        bool DeleteCustomer(int id);
        IEnumerable<Employee> GetAllEmployee();
        Employee GetEmployeeById(int id);
        Employee GetEmployeeReportsTo(int id);

        Employee AddEmployee(Employee newEmployee);

        bool UpdateEmployee(Employee employee);

        bool DeleteEmployee(int id);

        IEnumerable<Employee> GetEmployeeDirectReports(int id);

        IEnumerable<Employee> GetDirectReports(int id);
        IEnumerable<Genre> GetAllGenre();
        Genre GetGenreById(int id);

        Genre AddGenre(Genre newGenre);

        bool UpdateGenre(Genre genre);
        bool DeleteGenre(int id);
        IEnumerable<InvoiceLine> GetAllInvoiceLine();
        InvoiceLine GetInvoiceLineById(int id);

        IEnumerable<InvoiceLine> GetInvoiceLineByInvoiceId(int id);

        IEnumerable<InvoiceLine> GetInvoiceLineByTrackId(int id);

        InvoiceLine AddInvoiceLine(InvoiceLine newInvoiceLine);

        bool UpdateInvoiceLine(InvoiceLine invoiceLine);

        bool DeleteInvoiceLine(int id);
        IEnumerable<Invoice> GetAllInvoice();
        Invoice GetInvoiceById(int id);

        IEnumerable<Invoice> GetInvoiceByCustomerId(int id);

        Invoice AddInvoice(Invoice newInvoice);

        bool UpdateInvoice(Invoice invoice);

        bool DeleteInvoice(int id);
        
        IEnumerable<Invoice> GetInvoiceByEmployeeId(int id);
        
        IEnumerable<MediaType> GetAllMediaType();
        MediaType GetMediaTypeById(int id);

        MediaType AddMediaType(MediaType newMediaType);

        bool UpdateMediaType(MediaType mediaType);

        bool DeleteMediaType(int id);
        IEnumerable<Playlist> GetAllPlaylist();
        Playlist GetPlaylistById(int id);

        Playlist AddPlaylist(Playlist newPlaylist);

        bool UpdatePlaylist(Playlist playlist);

        bool DeletePlaylist(int id);
        
        IEnumerable<Playlist> GetPlaylistByTrackId(int id);
        
        IEnumerable<Track> GetAllTrack();
        Track GetTrackById(int id);
        IEnumerable<Track> GetTrackByAlbumId(int id);
        IEnumerable<Track> GetTrackByGenreId(int id);

        IEnumerable<Track>
            GetTrackByMediaTypeId(int id);

        IEnumerable<Track> GetTrackByPlaylistId(int id);

        Track AddTrack(Track newTrack);

        bool UpdateTrack(Track track);
        bool DeleteTrack(int id);
        
        IEnumerable<Track> GetTrackByArtistId(int id);
        IEnumerable<Track> GetTrackByInvoiceId(int id);
    }
}