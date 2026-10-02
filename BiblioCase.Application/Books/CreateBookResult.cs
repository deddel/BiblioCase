using BiblioCase.Application.DTOs;

namespace BiblioCase.Application.Books;

public sealed record CreateBookResult(BookDto? Book, bool AuthorNotFound = false);
