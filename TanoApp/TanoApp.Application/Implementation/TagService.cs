using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TanoApp.Application.Interfaces;
using TanoApp.Application.ViewModels.Common;
using TanoApp.Data.Entities;
using TanoApp.Data.IRepositories;
using TanoApp.Infrastructure.Interfaces;
using TeduCoreApp.Utilities.Dtos;

namespace TanoApp.Application.Implementation
{
    public class TagService : ITagService
    {
        private IMapper _mapper;
        private IUnitOfWork _unitOfWork;
        private ITagRepository _tagRepository;
        public TagService(ITagRepository tagRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public TagViewModel Add(TagViewModel tag)
        {
            _tagRepository.Add(_mapper.Map<TagViewModel, Tag>(tag));
            return tag;
        }

        public void Delete(string id)
        {
            _tagRepository.Remove(id);
        }

        public PagedResult<TagViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _tagRepository.FindAll();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }
            int totalRow = query.Count();
            query = query.OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize);
            var tags = query.ToList();
            var data = _mapper.Map<List<Tag>, List<TagViewModel>>(tags);
            var paginationSet = new PagedResult<TagViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public TagViewModel GetById(string id)
        {
            var tags =  _tagRepository.FindById(id);
            return _mapper.Map<Tag, TagViewModel>(tags);
        }

        public List<TagViewModel> GetListTag()
        {
            var tags = _tagRepository.FindAll().ToList();
            return _mapper.Map<List<Tag>, List<TagViewModel>>(tags);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(TagViewModel tag)
        {
            throw new NotImplementedException();
        }
    }
}
