﻿using ArabTube.Entities.Models;
using ArabTube.Services.DataServices.Data;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FFmpeg.NET.MetaData;

namespace ArabTube.Services.DataServices.Repositories.ImplementationClasses
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Comment>> GetVideoCommentsAsync(string videoId)
        {
            var comments = await _dbSet.Where(c => c.VideoId == videoId && c.ParentCommentId == c.Id)
                                    .Include(c => c.Childrens).ThenInclude(c => c.AppUser).ToListAsync();
            return comments;
        }

        public async Task<IEnumerable<Comment>> GetCommentAsync(string commentId)
        {
            var comments = await _dbSet.Where(c => c.Id == commentId)
                                    .Include(c => c.Childrens).ThenInclude(c => c.AppUser).ToListAsync();
            return comments;
        }

        public void DeleteComment(Comment comment)
        {
            _dbSet.Remove(comment);
        }

        public async Task DeleteCommentAsync(string commentId)
        {
            var childrens = await _dbSet.Where(c => c.ParentCommentId == commentId).ToListAsync();
            _dbSet.RemoveRange(childrens);
            var parent = await _dbSet.FirstOrDefaultAsync(c => c.Id == commentId);
            _dbSet.Remove(parent);
        }

        public async Task DeleteVideoCommentsAsync(string videoId)
        {
            var comments = await _dbSet.Where(c => c.VideoId == videoId && c.Id != c.ParentCommentId).ToListAsync();
            _dbSet.RemoveRange(comments);
            comments = await _dbSet.Where(c => c.VideoId == videoId).ToListAsync();
            _dbSet.RemoveRange(comments);
        }
    }
}