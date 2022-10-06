using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;

public class MessageBoxUsc_MsgBoxUsc : UserControl
{
	public enum enmAnswer
	{
		OK,
		Cancel
	}

	public enum enmMessageType
	{
		Error,
		Success,
		Attention,
		Info
	}

	public class MsgBoxEventArgs : EventArgs
	{
		public enmAnswer Answer;

		public string Args;

		public MsgBoxEventArgs(enmAnswer answer, string args)
		{
			Answer = answer;
			Args = args;
		}
	}

	public delegate void MsgBoxEventHandler(object sender, MsgBoxEventArgs e);

	public class Message
	{
		private enmMessageType _messageType = enmMessageType.Info;

		private string _messageText = "";

		public enmMessageType MessageType
		{
			get
			{
				return _messageType;
			}
			set
			{
				_messageType = value;
			}
		}

		public string MessageText
		{
			get
			{
				return _messageText;
			}
			set
			{
				_messageText = value;
			}
		}

		public Message(string messageText, enmMessageType messageType)
		{
			_messageText = messageText;
			_messageType = messageType;
		}
	}

	private List<Message> Messages = new List<Message>();

	protected UpdatePanel udpMsj;

	protected Button btnD;

	protected Button btnD2;

	protected Panel pnlMsg;

	protected Panel pnlMsgHD;

	protected GridView grvMsg;

	protected Button btnOK;

	protected Button btnPostOK;

	protected Button btnPostCancel;

	protected ModalPopupExtender mpeMsg;

	public string Args
	{
		get
		{
			if (ViewState["Args"] == null)
			{
				ViewState["Args"] = "";
			}
			return Convert.ToString(ViewState["Args"]);
		}
		set
		{
			ViewState["Args"] = value;
		}
	}

	protected int MessageNumber 
    {
        get
        {
            return Messages.Count;
        }
    }

	public event MsgBoxEventHandler MsgBoxAnswered;

	public void AddMessage(string msgText, enmMessageType type)
	{
		Messages.Add(new Message(msgText, type));
		Args = "";
		btnPostCancel.Visible = false;
		btnPostOK.Visible = false;
		btnOK.Visible = true;
	}

	public void AddMessage(string msgText, enmMessageType type, bool postPage, bool showCancelButton, string args)
	{
		Messages.Add(new Message(msgText, type));
		if (!string.IsNullOrEmpty(args))
		{
			Args = args;
		}
		btnPostCancel.Visible = showCancelButton;
		btnPostOK.Visible = postPage;
		btnOK.Visible = !postPage;
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		if (!btnOK.Visible)
		{
			mpeMsg.OkControlID = "btnD2";
		}
		else
		{
			mpeMsg.OkControlID = "btnOK";
		}
		if (Messages.Count > 0)
		{
			btnOK.Focus();
			grvMsg.DataSource = Messages;
			grvMsg.DataBind();
			mpeMsg.OnShown = null;
			mpeMsg.Show();
			udpMsj.Update();
		}
		else
		{
			grvMsg.DataBind();
			udpMsj.Update();
		}
		if (Parent.GetType() == typeof(UpdatePanel))
		{
			UpdatePanel updatePanel = Parent as UpdatePanel;
			updatePanel.Update();
		}
	}

	protected void btnPostOK_Click(object sender, EventArgs e)
	{
		if (this.MsgBoxAnswered != null)
		{
			this.MsgBoxAnswered(this, new MsgBoxEventArgs(enmAnswer.OK, Args));
			Args = "";
		}
	}

	protected void btnPostCancel_Click(object sender, EventArgs e)
	{
		if (this.MsgBoxAnswered != null)
		{
			this.MsgBoxAnswered(this, new MsgBoxEventArgs(enmAnswer.Cancel, Args));
			Args = "";
		}
	}
}
