//using System;
//using System.Linq;
//using Business.DTOs.Tag;
//using Core.Entities;
//using Data.Context;

//public class TagService
//{
//    private readonly AppDbContext _context;

//    public TagService(AppDbContext context)
//    {
//        _context = context;
//    }

//    // This method creates a new tag and returns the created tag
//    public CreateTagDTO CreateTag(string tagName)
//    {
//        if (string.IsNullOrEmpty(tagName)) return null;

//        var tag = new Tag
//        {
//            Id = Guid.NewGuid(),
//            Name = tagName,
//            IsDeleted = false
//        };

//        _context.Tags.Add(tag);
//        _context.SaveChanges(); // Save the new tag in the database

//        return new CreateTagDTO
//        {
//            Id = tag.Id,
//            Name = tag.Name
//        };
//    }

//    // This method gets all tags from the database (you can modify it to fit your needs)
//    public IQueryable<CreateTagDTO> GetAllTags()
//    {
//        return _context.Tags
//            .Where(t => !t.IsDeleted)
//            .Select(t => new CreateTagDTO
//            {
//                Id = t.Id,
//                Name = t.Name
//            });
//    }
//}
