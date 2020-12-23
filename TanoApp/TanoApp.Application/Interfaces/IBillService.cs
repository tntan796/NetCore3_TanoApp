using System;
using System.Collections.Generic;
using System.Text;
using TanoApp.Application.ViewModels.Products;
using TanoApp.Data.Entities;
using TanoApp.Data.Enums;
using TeduCoreApp.Utilities.Dtos;

namespace TanoApp.Application.Interfaces
{
    public interface IBillService
    {
        void Create(BillViewModel billVm);
        void Update(BillViewModel billVm);
        PagedResult<BillViewModel> GetAllPaging(string startDate, string endDate, string keyWord, int pageIndex, int pageSize);
        BillViewModel GetDetail(int billId);
        BillDetailViewModel CreateBillDetail(BillDetailViewModel billDetailVm);
        void DeleteDetail(int productId, int billId, int colorId, int sizeId);
        void UpdateStatus(int orderId, BillStatus status);
        List<BillDetailViewModel> GetBillDetails(int billId);
        List<ColorViewModel> GetColors();
        List<SizeViewModel> GetSizes();
        void Save();

    }
}
