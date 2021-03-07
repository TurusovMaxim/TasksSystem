using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksSystem.Models;
using TasksSystem.Repos;

namespace TasksSystem.Services
{
    public class CommentService
    {
        private readonly CommentRepository _commentRepo;

        public CommentService(CommentRepository commentRepository)
        {
            _commentRepo = commentRepository;
        }

        public async System.Threading.Tasks.Task CreateComment(Comment comment, string userEmail, int id)
        {
            await _commentRepo.CreateCommentAsync(comment, userEmail, id);
        }

        public async System.Threading.Tasks.Task<List<Comment>> GetAllComments()
        {
            return await _commentRepo.GetAllComments();
        }

        public async System.Threading.Tasks.Task<List<Comment>> GetAllTasksComments(int taskId)
        {
            return await _commentRepo.GetAllTasksComments(taskId);
        }

        public async System.Threading.Tasks.Task<Comment> GetCommentById(int? id)
        {
            return await _commentRepo.GetCommentById(id);
        }

        public async System.Threading.Tasks.Task RemoveComment(Comment comment)
        {
            await _commentRepo.RemoveComment(comment);
        }
    }
}
