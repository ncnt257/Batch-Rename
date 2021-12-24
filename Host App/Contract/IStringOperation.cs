using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace Contract
{
    public interface IStringOperation : INotifyPropertyChanged
    {
        //nhớ cài thêm constructor cho các luật, chỗ này xài abstract class có vẻ hợp lý hơn nhưng mà lỡ rồi
        public delegate void Trigger();
        public event Trigger PreviewTriggerEvent;
        public StringArgs Args { get; set; }
        public string Name { get; }
        public string Description { get; }
        public UserControl ConfigUC { get; set; }
        public bool IsChecked { get; set; }
        public bool IsValidParams { get; }//Đã đủ tham số để operate hay chưa??
        public IStringOperation Clone();
        public string Operate(string origin, bool isFile);
        public void CreateFromRaw(RawRule input);
        public List<string> GetStringAgrs();
        public void DescriptionChangedNotify();
        public void ResetRule();//Một số luật cần được reset sau khi đổi tên (Tùng bị dính luật add counter kb luật của ae bị không, nếu không cần thì cứ khai báo hàm, để không cũng được )

    }
}
