using AutoMapper;
using ClassLibrary.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksSystem.Models;

namespace TasksSystem.Repos
{
    public class CommentRepository
    {
        private readonly ClassLibraryContext _context;
        private readonly IMapper _mapper;
        public CommentRepository(ClassLibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async System.Threading.Tasks.Task CreateCommentAsync(Comment comment, string userEmail, int id)
        {
            var task = await _context.Task.FirstOrDefaultAsync(t => t.Id == id);
            var user = await _context.User.FirstOrDefaultAsync(c => c.Email.Equals(userEmail));
            var commentDb = _mapper.Map<Comment, CommentDb>(comment);
            commentDb.User = user;
            commentDb.Task = task;
            _context.Comments.Add(commentDb);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task<List<Comment>> GetAllComments()
        {
            var comments = _mapper.Map<List<CommentDb>, List<Comment>>(await _context.Comments.AsNoTracking().ToListAsync());
            return comments;
        }

        public async System.Threading.Tasks.Task<List<Comment>> GetAllTasksComments(int taskId)
        {
            var task = await _context.Task.FirstOrDefaultAsync(t => t.Id == taskId);
            var commentsDb = await _context.Comments.Include(c => c.User).
                                    Where(c => c.Task.Equals(task)).
                                    AsNoTracking().ToListAsync();
            var comments = _mapper.Map<List<CommentDb>, List<Comment>>(commentsDb);
            return comments;
        }

        public async System.Threading.Tasks.Task<Comment> GetCommentById(int? id)
        {
            var comment = await _context.Comments.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<CommentDb, Comment>(comment);
        }

        public async System.Threading.Tasks.Task RemoveComment(Comment comment)
        {
            _context.Comments.Remove(_mapper.Map<Comment, CommentDb>(comment));
            await _context.SaveChangesAsync();
        }
    }
}
