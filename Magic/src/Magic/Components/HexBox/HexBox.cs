using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Security.Permissions;
using System.Windows.Forms.VisualStyles;
using System.Text;
using System.Collections.Generic;

namespace Magic.Components
{
	/// <summary>
	/// Represents a hex box control.
	/// </summary>
	public class HexBox : Control
	{
		#region Fields

		/// <summary>
		/// Contains the hole content bounds of all text
		/// </summary>
		Rectangle _recContent;
		/// <summary>
		/// Contains the line info bounds
		/// </summary>
		Rectangle _recLineInfo;
		/// <summary>
		/// Contains the column info header rectangle bounds
		/// </summary>
		Rectangle _recColumnInfo;
		/// <summary>
		/// Contains the hex data bounds
		/// </summary>
		Rectangle _recHex;
		/// <summary>
		/// Contains the string view bounds
		/// </summary>
		Rectangle _recStringView;

		/// <summary>
		/// Contains string format information for text drawing
		/// </summary>
		StringFormat _stringFormat;
        /// <summary>
        /// Contains the maximum of visible horizontal bytes
        /// </summary>
        Int32 _iHexMaxHBytes;
        /// <summary>
        /// Contains the maximum of visible vertical bytes
        /// </summary>
        Int32 _iHexMaxVBytes;
        /// <summary>
        /// Contains the maximum of visible bytes.
        /// </summary>
        Int32 _iHexMaxBytes;

        /// <summary>
        /// Contains the scroll bars minimum value
        /// </summary>
        Int64 _scrollVmin;
        /// <summary>
        /// Contains the scroll bars maximum value
        /// </summary>
        Int64 _scrollVmax;
        /// <summary>
        /// Contains the scroll bars current position
        /// </summary>
        Int64 _scrollVpos;
		/// <summary>
		/// Contains a vertical scroll
		/// </summary>
		VScrollBar _vScrollBar;
		/// <summary>
		/// Contains a timer for thumbtrack scrolling
		/// </summary>
		Timer _thumbTrackTimer = new Timer();
        /// <summary>
        /// Contains the thumbtrack scrolling position
        /// </summary>
        Int64 _thumbTrackPosition;
		/// <summary>
		/// Contains the thumptrack delay for scrolling in milliseconds.
		/// </summary>
		const Int32 THUMPTRACKDELAY = 50;
        /// <summary>
        /// Contains the Enviroment.TickCount of the last refresh
        /// </summary>
        Int32 _lastThumbtrack;
        /// <summary>
        /// Contains the border큦 left shift
        /// </summary>
        Int32 _recBorderLeft = SystemInformation.Border3DSize.Width;
        /// <summary>
        /// Contains the border큦 right shift
        /// </summary>
        Int32 _recBorderRight = SystemInformation.Border3DSize.Width;
        /// <summary>
        /// Contains the border큦 top shift
        /// </summary>
        Int32 _recBorderTop = SystemInformation.Border3DSize.Height;
        /// <summary>
        /// Contains the border bottom shift
        /// </summary>
        Int32 _recBorderBottom = SystemInformation.Border3DSize.Height;

        /// <summary>
        /// Contains the index of the first visible byte
        /// </summary>
        Int64 _startByte;
        /// <summary>
        /// Contains the index of the last visible byte
        /// </summary>
        Int64 _endByte;

        /// <summary>
        /// Contains the current byte position
        /// </summary>
        Int64 _bytePos = -1;
        /// <summary>
        /// Contains the current char position in one byte
        /// </summary>
        /// <example>
        /// "1A"
        /// "1" = char position of 0
        /// "A" = char position of 1
        /// </example>
        Int32 _byteCharacterPos;

        /// <summary>
        /// Contains string format information for hex values
        /// </summary>
        String _hexStringFormat = "X";


		/// <summary>
		/// Contains the current key interpreter
		/// </summary>
		IKeyInterpreter _keyInterpreter;
		/// <summary>
		/// Contains an empty key interpreter without functionality
		/// </summary>
		EmptyKeyInterpreter _eki;
		/// <summary>
		/// Contains the default key interpreter
		/// </summary>
		KeyInterpreter _ki;
		/// <summary>
		/// Contains the string key interpreter
		/// </summary>
		StringKeyInterpreter _ski;

        /// <summary>
        /// Contains True if caret is visible
        /// </summary>
        Boolean _caretVisible;

        /// <summary>
        /// Contains true, if the find (Find method) should be aborted.
        /// </summary>
        Boolean _abortFind;
        /// <summary>
        /// Contains a value of the current finding position.
        /// </summary>
        Int64 _findingPos;

        /// <summary>
        /// Contains a state value about Insert or Write mode. When this value is true and the ByteProvider SupportsInsert is true bytes are inserted instead of overridden.
        /// </summary>
        Boolean _insertActive;
        #endregion
        
        #region Properties

        /// <summary>
        /// Gets or sets the background color for the disabled control.
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Color), "WhiteSmoke")]
        public Color BackColorDisabled
        {
            get
            {
                return this._backColorDisabled;
            }
            set
            {
                this._backColorDisabled = value;
            }
        }
        Color _backColorDisabled = Color.FromName("WhiteSmoke");

        /// <summary>
        /// Gets or sets if the count of bytes in one line is fix.
        /// </summary>
        /// <remarks>
        /// When set to True, BytesPerLine property determine the maximum count of bytes in one line.
        /// </remarks>
        [DefaultValue(false), Category("Hex"), Description("Gets or sets if the count of bytes in one line is fix.")]
        public Boolean ReadOnly
        {
            get { return this._readOnly; }
            set
            {
                if (this._readOnly == value)
                    return;

                this._readOnly = value;
                this.OnReadOnlyChanged(EventArgs.Empty);
                this.Invalidate();
            }
        }
        Boolean _readOnly;

        /// <summary>
        /// Gets or sets the maximum count of bytes in one line.
        /// </summary>
        /// <remarks>
        /// UseFixedBytesPerLine property no longer has to be set to true for this to work
        /// </remarks>
        [DefaultValue(16), Category("Hex"), Description("Gets or sets the maximum count of bytes in one line.")]
        public Int32 BytesPerLine
        {
            get { return this._bytesPerLine; }
            set
            {
                if (this._bytesPerLine == value)
                    return;

                this._bytesPerLine = value;
                this.OnBytesPerLineChanged(EventArgs.Empty);

                this.UpdateRectanglePositioning();
                this.Invalidate();
            }
        }
        Int32 _bytesPerLine = 16;

        /// <summary>
        /// Gets or sets the number of bytes in a group. Used to show the group separator line (if GroupSeparatorVisible is true)
        /// </summary>
        /// <remarks>
        /// GroupSeparatorVisible property must set to true
        /// </remarks>
        [DefaultValue(4), Category("Hex"), Description("Gets or sets the byte-count between group separators (if visible).")]
        public Int32 GroupSize
        {
            get { return this._groupSize; }
            set
            {
                if (this._groupSize == value)
                    return;

                this._groupSize = value;
                this.OnGroupSizeChanged(EventArgs.Empty);

                this.UpdateRectanglePositioning();
                this.Invalidate();
            }
        }
        Int32 _groupSize = 4;
        /// <summary>
        /// Gets or sets if the count of bytes in one line is fix.
        /// </summary>
        /// <remarks>
        /// When set to True, BytesPerLine property determine the maximum count of bytes in one line.
        /// </remarks>
        [DefaultValue(false), Category("Hex"), Description("Gets or sets if the count of bytes in one line is fix.")]
        public Boolean UseFixedBytesPerLine
        {
            get { return this._useFixedBytesPerLine; }
            set
            {
                if (this._useFixedBytesPerLine == value)
                    return;

                this._useFixedBytesPerLine = value;
                this.OnUseFixedBytesPerLineChanged(EventArgs.Empty);

                this.UpdateRectanglePositioning();
                this.Invalidate();
            }
        }
        Boolean _useFixedBytesPerLine;

        /// <summary>
        /// Gets or sets the visibility of a vertical scroll bar.
        /// </summary>
        [DefaultValue(false), Category("Hex"), Description("Gets or sets the visibility of a vertical scroll bar.")]
        public Boolean VScrollBarVisible
        {
            get { return this._vScrollBarVisible; }
            set
            {
                if (this._vScrollBarVisible == value)
                    return;

                this._vScrollBarVisible = value;

                if (this._vScrollBarVisible)
                    this.Controls.Add(this._vScrollBar);
                else
                    this.Controls.Remove(this._vScrollBar);

                this.UpdateRectanglePositioning();
                this.UpdateScrollSize();

                this.OnVScrollBarVisibleChanged(EventArgs.Empty);
            }
        }
        Boolean _vScrollBarVisible;

        /// <summary>
        /// Gets or sets the ByteProvider.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IByteProvider ByteProvider
        {
            get { return this._byteProvider; }
            set
            {
                if (this._byteProvider == value)
                    return;

                if (value == null)
                    this.ActivateEmptyKeyInterpreter();
                else
                    this.ActivateKeyInterpreter();

                if (this._byteProvider != null)
                    this._byteProvider.LengthChanged -= new EventHandler(this._byteProvider_LengthChanged);

                this._byteProvider = value;
                if (this._byteProvider != null)
                    this._byteProvider.LengthChanged += new EventHandler(this._byteProvider_LengthChanged);

                this.OnByteProviderChanged(EventArgs.Empty);

                if (value == null) // do not raise events if value is null
                {
                    this._bytePos = -1;
                    this._byteCharacterPos = 0;
                    this._selectionLength = 0;

                    this.DestroyCaret();
                }
                else
                {
                    this.SetPosition(0, 0);
                    this.SetSelectionLength(0);

                    if (this._caretVisible && this.Focused)
                        this.UpdateCaret();
                    else
                        this.CreateCaret();
                }

                this.CheckCurrentLineChanged();
                this.CheckCurrentPositionInLineChanged();

                this._scrollVpos = 0;

                this.UpdateVisibilityBytes();
                this.UpdateRectanglePositioning();

                this.Invalidate();
            }
        }

        IByteProvider _byteProvider;
        /// <summary>
        /// Gets or sets the visibility of the group separator.
        /// </summary>
        [DefaultValue(false), Category("Hex"), Description("Gets or sets the visibility of a separator vertical line.")]
        public Boolean GroupSeparatorVisible
        {
            get { return this._groupSeparatorVisible; }
            set
            {
                if (this._groupSeparatorVisible == value)
                    return;

                this._groupSeparatorVisible = value;
                this.OnGroupSeparatorVisibleChanged(EventArgs.Empty);

                this.UpdateRectanglePositioning();
                this.Invalidate();
            }
        }
        Boolean _groupSeparatorVisible = false;

        /// <summary>
        /// Gets or sets the visibility of the column info
        /// </summary>
        [DefaultValue(false), Category("Hex"), Description("Gets or sets the visibility of header row.")]
        public Boolean ColumnInfoVisible
        {
            get { return this._columnInfoVisible; }
            set
            {
                if (this._columnInfoVisible == value)
                    return;

                this._columnInfoVisible = value;
                this.OnColumnInfoVisibleChanged(EventArgs.Empty);

                this.UpdateRectanglePositioning();
                this.Invalidate();
            }
        }
        Boolean _columnInfoVisible = false;

        /// <summary>
        /// Gets or sets the visibility of a line info.
        /// </summary>
        [DefaultValue(false), Category("Hex"), Description("Gets or sets the visibility of a line info.")]
        public Boolean LineInfoVisible
        {
            get { return this._lineInfoVisible; }
            set
            {
                if (this._lineInfoVisible == value)
                    return;

                this._lineInfoVisible = value;
                this.OnLineInfoVisibleChanged(EventArgs.Empty);

                this.UpdateRectanglePositioning();
                this.Invalidate();
            }
        }
        Boolean _lineInfoVisible = false;

        /// <summary>
        /// Gets or sets the offset of a line info.
        /// </summary>
        [DefaultValue((Int64)0), Category("Hex"), Description("Gets or sets the offset of the line info.")]
        public Int64 LineInfoOffset
        {
            get { return this._lineInfoOffset; }
            set
            {
                if (this._lineInfoOffset == value)
                    return;

                this._lineInfoOffset = value;

                this.Invalidate();
            }
        }
        Int64 _lineInfoOffset = 0;

        /// <summary>
        /// Gets or sets the hex box큦 border style.
        /// </summary>
        [DefaultValue(typeof(BorderStyle), "Fixed3D"), Category("Hex"), Description("Gets or sets the hex box큦 border style.")]
        public BorderStyle BorderStyle
        {
            get { return this._borderStyle; }
            set
            {
                if (this._borderStyle == value)
                    return;

                this._borderStyle = value;
                switch (this._borderStyle)
                {
                    case BorderStyle.None:
                        this._recBorderLeft = this._recBorderTop = this._recBorderRight = this._recBorderBottom = 0;
                        break;
                    case BorderStyle.Fixed3D:
                        this._recBorderLeft = this._recBorderRight = SystemInformation.Border3DSize.Width;
                        this._recBorderTop = this._recBorderBottom = SystemInformation.Border3DSize.Height;
                        break;
                    case BorderStyle.FixedSingle:
                        this._recBorderLeft = this._recBorderTop = this._recBorderRight = this._recBorderBottom = 1;
                        break;
                }

                this.UpdateRectanglePositioning();

                this.OnBorderStyleChanged(EventArgs.Empty);

            }
        }
        BorderStyle _borderStyle = BorderStyle.Fixed3D;

        /// <summary>
        /// Gets or sets the visibility of the string view.
        /// </summary>
        [DefaultValue(false), Category("Hex"), Description("Gets or sets the visibility of the string view.")]
        public Boolean StringViewVisible
        {
            get { return this._stringViewVisible; }
            set
            {
                if (this._stringViewVisible == value)
                    return;

                this._stringViewVisible = value;
                this.OnStringViewVisibleChanged(EventArgs.Empty);

                this.UpdateRectanglePositioning();
                this.Invalidate();
            }
        }
        Boolean _stringViewVisible;

        /// <summary>
        /// Gets or sets whether the HexBox control displays the hex characters in upper or lower case.
        /// </summary>
        [DefaultValue(typeof(HexCasing), "Upper"), Category("Hex"), Description("Gets or sets whether the HexBox control displays the hex characters in upper or lower case.")]
        public HexCasing HexCasing
        {
            get
            {
                if (this._hexStringFormat == "X")
                    return HexCasing.Upper;
                else
                    return HexCasing.Lower;
            }
            set
            {
                String format;
                if (value == HexCasing.Upper)
                    format = "X";
                else
                    format = "x";

                if (this._hexStringFormat == format)
                    return;

                this._hexStringFormat = format;
                this.OnHexCasingChanged(EventArgs.Empty);

                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets and sets the starting point of the bytes selected in the hex box.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Int64 SelectionStart
        {
            get { return this._bytePos; }
            set
            {
                this.SetPosition(value, 0);
                this.ScrollByteIntoView();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets and sets the number of bytes selected in the hex box.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Int64 SelectionLength
        {
            get { return this._selectionLength; }
            set
            {
                this.SetSelectionLength(value);
                this.ScrollByteIntoView();
                this.Invalidate();
            }
        }
        Int64 _selectionLength;


        /// <summary>
        /// Gets or sets the info color used for column info and line info. When this property is null, then ForeColor property is used.
        /// </summary>
        [DefaultValue(typeof(Color), "Gray"), Category("Hex"), Description("Gets or sets the line info color. When this property is null, then ForeColor property is used.")]
        public Color InfoForeColor
        {
            get { return this._infoForeColor; }
            set { this._infoForeColor = value; this.Invalidate(); }
        }
        Color _infoForeColor = Color.Gray;

        /// <summary>
        /// Gets or sets the background color for the selected bytes.
        /// </summary>
        [DefaultValue(typeof(Color), "Blue"), Category("Hex"), Description("Gets or sets the background color for the selected bytes.")]
        public Color SelectionBackColor
        {
            get { return this._selectionBackColor; }
            set { this._selectionBackColor = value; this.Invalidate(); }
        }
        Color _selectionBackColor = SystemColors.Highlight;

        /// <summary>
        /// Gets or sets the foreground color for the selected bytes.
        /// </summary>
        [DefaultValue(typeof(Color), "White"), Category("Hex"), Description("Gets or sets the color of the text for the selected bytes.")]
        public Color SelectionForeColor
        {
            get { return this._selectionForeColor; }
            set { this._selectionForeColor = value; this.Invalidate(); }
        }
        Color _selectionForeColor = Color.White;

        /// <summary>
        /// Gets or sets the visibility of a shadow selection.
        /// </summary>
        [DefaultValue(true), Category("Hex"), Description("Gets or sets the visibility of a shadow selection.")]
        public Boolean ShadowSelectionVisible
        {
            get { return this._shadowSelectionVisible; }
            set
            {
                if (this._shadowSelectionVisible == value)
                    return;
                this._shadowSelectionVisible = value;
                this.Invalidate();
            }
        }
        Boolean _shadowSelectionVisible = true;

        /// <summary>
        /// Gets or sets the color of the shadow selection. 
        /// </summary>
        /// <remarks>
        /// A alpha component must be given! 
        /// Default alpha = 100
        /// </remarks>
        [Category("Hex"), Description("Gets or sets the color of the shadow selection.")]
        public Color ShadowSelectionColor
        {
            get { return this._shadowSelectionColor; }
            set { this._shadowSelectionColor = value; this.Invalidate(); }
        }
        Color _shadowSelectionColor = Color.FromArgb(100, Color.SkyBlue);

        /// <summary>
        /// Contains the size of a single character in pixel
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SizeF CharSize
        {
            get { return this._charSize; }
            private set
            {
                if (this._charSize == value)
                    return;
                this._charSize = value;
                if (CharSizeChanged != null)
                    CharSizeChanged(this, EventArgs.Empty);
            }
        }
        SizeF _charSize;

        /// <summary>
        /// Gets the width required for the content
        /// </summary>
        [DefaultValue(0), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Int32 RequiredWidth
        {
            get { return this._requiredWidth; }
            private set
            {
                if (this._requiredWidth == value)
                    return;
                this._requiredWidth = value;
                if (RequiredWidthChanged != null)
                    RequiredWidthChanged(this, EventArgs.Empty);
            }
        }
        Int32 _requiredWidth;

        /// <summary>
        /// Gets the number bytes drawn horizontally.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Int32 HorizontalByteCount
        {
            get { return this._iHexMaxHBytes; }
        }

        /// <summary>
        /// Gets the number bytes drawn vertically.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Int32 VerticalByteCount
        {
            get { return this._iHexMaxVBytes; }
        }

        /// <summary>
        /// Gets the current line
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Int64 CurrentLine
        {
            get { return this._currentLine; }
        }
        Int64 _currentLine;

        /// <summary>
        /// Gets the current position in the current line
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Int64 CurrentPositionInLine
        {
            get { return this._currentPositionInLine; }
        }
        Int32 _currentPositionInLine;

        /// <summary>
        /// Gets the a value if insertion mode is active or not.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Boolean InsertActive
        {
            get { return this._insertActive; }
            set
            {
                if (this._insertActive == value)
                    return;

                this._insertActive = value;

                // recreate caret
                this.DestroyCaret();
                this.CreateCaret();

                // raise change event
                this.OnInsertActiveChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the built-in context menu.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BuiltInContextMenu BuiltInContextMenu
        {
            get { return this._builtInContextMenu; }
        }
        BuiltInContextMenu _builtInContextMenu;


        /// <summary>
        /// Gets or sets the converter that will translate between byte and character values.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IByteCharConverter ByteCharConverter
        {
            get
            {
                if (this._byteCharConverter == null)
                    this._byteCharConverter = new DefaultByteCharConverter();
                return this._byteCharConverter;
            }
            set
            {
                if (value != null && value != this._byteCharConverter)
                {
                    this._byteCharConverter = value;
                    this.Invalidate();
                }
            }
        }
        IByteCharConverter _byteCharConverter;

        #endregion

        #region Overridden properties
        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        [DefaultValue(typeof(Color), "White")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        /// <summary>
        /// The font used to display text in the hexbox.
        /// </summary>
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                if (value == null)
                    return;

                base.Font = value;
                this.UpdateRectanglePositioning();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Read-only, gets the string of spaced hex currently in the hexbox.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public override String Text
        {
            get
            {
                this._text = this.ConvertBytesToHex(this.Value);
                return this._text;
            }
        }
        private String _text;

        /// <summary>
        /// Gets or sets the byte array contained in the hexbox.
        /// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public Byte[] Value
        {
            get
            {
                this._value = ((DataByteProvider)this.ByteProvider).Bytes.ToArray();
                return this._value;
            }
            set
            {
                this._value = value;
                this.ByteProvider = new DataByteProvider(this._value);
                return;
            }
        }
        private Byte[] _value;

        /// <summary>
        /// Not used.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never), Bindable(false)]
        public override RightToLeft RightToLeft
        {
            get
            {
                return base.RightToLeft;
            }
            set
            {
                base.RightToLeft = value;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs, when the value of InsertActive property has changed.
        /// </summary>
        [Description("Occurs, when the value of InsertActive property has changed.")]
		public event EventHandler InsertActiveChanged;
		/// <summary>
		/// Occurs, when the value of ReadOnly property has changed.
		/// </summary>
		[Description("Occurs, when the value of ReadOnly property has changed.")]
		public event EventHandler ReadOnlyChanged;
		/// <summary>
		/// Occurs, when the value of ByteProvider property has changed.
		/// </summary>
		[Description("Occurs, when the value of ByteProvider property has changed.")]
		public event EventHandler ByteProviderChanged;
		/// <summary>
		/// Occurs, when the value of SelectionStart property has changed.
		/// </summary>
		[Description("Occurs, when the value of SelectionStart property has changed.")]
		public event EventHandler SelectionStartChanged;
		/// <summary>
		/// Occurs, when the value of SelectionLength property has changed.
		/// </summary>
		[Description("Occurs, when the value of SelectionLength property has changed.")]
		public event EventHandler SelectionLengthChanged;
		/// <summary>
		/// Occurs, when the value of LineInfoVisible property has changed.
		/// </summary>
		[Description("Occurs, when the value of LineInfoVisible property has changed.")]
		public event EventHandler LineInfoVisibleChanged;
		/// <summary>
		/// Occurs, when the value of ColumnInfoVisibleChanged property has changed.
		/// </summary>
		[Description("Occurs, when the value of ColumnInfoVisibleChanged property has changed.")]
		public event EventHandler ColumnInfoVisibleChanged;
		/// <summary>
		/// Occurs, when the value of GroupSeparatorVisibleChanged property has changed.
		/// </summary>
		[Description("Occurs, when the value of GroupSeparatorVisibleChanged property has changed.")]
		public event EventHandler GroupSeparatorVisibleChanged;
		/// <summary>
		/// Occurs, when the value of StringViewVisible property has changed.
		/// </summary>
		[Description("Occurs, when the value of StringViewVisible property has changed.")]
		public event EventHandler StringViewVisibleChanged;
		/// <summary>
		/// Occurs, when the value of BorderStyle property has changed.
		/// </summary>
		[Description("Occurs, when the value of BorderStyle property has changed.")]
		public event EventHandler BorderStyleChanged;
		/// <summary>
		/// Occurs, when the value of ColumnWidth property has changed.
		/// </summary>
		[Description("Occurs, when the value of GroupSize property has changed.")]
		public event EventHandler GroupSizeChanged;
		/// <summary>
		/// Occurs, when the value of BytesPerLine property has changed.
		/// </summary>
		[Description("Occurs, when the value of BytesPerLine property has changed.")]
		public event EventHandler BytesPerLineChanged;
		/// <summary>
		/// Occurs, when the value of UseFixedBytesPerLine property has changed.
		/// </summary>
		[Description("Occurs, when the value of UseFixedBytesPerLine property has changed.")]
		public event EventHandler UseFixedBytesPerLineChanged;
		/// <summary>
		/// Occurs, when the value of VScrollBarVisible property has changed.
		/// </summary>
		[Description("Occurs, when the value of VScrollBarVisible property has changed.")]
		public event EventHandler VScrollBarVisibleChanged;
		/// <summary>
		/// Occurs, when the value of HexCasing property has changed.
		/// </summary>
		[Description("Occurs, when the value of HexCasing property has changed.")]
		public event EventHandler HexCasingChanged;
		/// <summary>
		/// Occurs, when the value of HorizontalByteCount property has changed.
		/// </summary>
		[Description("Occurs, when the value of HorizontalByteCount property has changed.")]
		public event EventHandler HorizontalByteCountChanged;
		/// <summary>
		/// Occurs, when the value of VerticalByteCount property has changed.
		/// </summary>
		[Description("Occurs, when the value of VerticalByteCount property has changed.")]
		public event EventHandler VerticalByteCountChanged;
		/// <summary>
		/// Occurs, when the value of CurrentLine property has changed.
		/// </summary>
		[Description("Occurs, when the value of CurrentLine property has changed.")]
		public event EventHandler CurrentLineChanged;
		/// <summary>
		/// Occurs, when the value of CurrentPositionInLine property has changed.
		/// </summary>
		[Description("Occurs, when the value of CurrentPositionInLine property has changed.")]
		public event EventHandler CurrentPositionInLineChanged;
		/// <summary>
		/// Occurs, when Copy method was invoked and ClipBoardData changed.
		/// </summary>
		[Description("Occurs, when Copy method was invoked and ClipBoardData changed.")]
		public event EventHandler Copied;
		/// <summary>
		/// Occurs, when CopyHex method was invoked and ClipBoardData changed.
		/// </summary>
		[Description("Occurs, when CopyHex method was invoked and ClipBoardData changed.")]
		public event EventHandler CopiedHex;
        /// <summary>
        /// Occurs, when the CharSize property has changed
        /// </summary>
        [Description("Occurs, when the CharSize property has changed")]
        public event EventHandler CharSizeChanged;
        /// <summary>
        /// Occurs, when the RequiredWidth property changes
        /// </summary>
        [Description("Occurs, when the RequiredWidth property changes")]
        public event EventHandler RequiredWidthChanged;
		#endregion



		#region Ctors

		/// <summary>
		/// Initializes a new instance of a HexBox class.
		/// </summary>
		public HexBox()
		{
			this._vScrollBar = new VScrollBar();
			this._vScrollBar.Scroll += new ScrollEventHandler(this._vScrollBar_Scroll);

			this._builtInContextMenu = new BuiltInContextMenu(this);
            this._builtInContextMenu.CopyMenuItemText = "Copy";
            this._builtInContextMenu.CutMenuItemText = "Cut";
            this._builtInContextMenu.PasteMenuItemText = "Paste";
            this._builtInContextMenu.SelectAllMenuItemText = "Select All";

            this.BackColor = Color.White;
            this.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, ((Byte)(0)));
            this._stringFormat = new StringFormat(StringFormat.GenericTypographic);
            this._stringFormat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;

            this.ActivateEmptyKeyInterpreter();

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            this._thumbTrackTimer.Interval = 50;
            this._thumbTrackTimer.Tick += new EventHandler(this.PerformScrollThumbTrack);
		}

        #endregion



        #region Caret methods
        void CreateCaret()
        {
            if (this._byteProvider == null || this._keyInterpreter == null || this._caretVisible || !this.Focused)
                return;

            System.Diagnostics.Debug.WriteLine("CreateCaret()", "HexBox");

            // define the caret width depending on InsertActive mode
            Int32 caretWidth = (this.InsertActive) ? 1 : (Int32)this._charSize.Width;
            Int32 caretHeight = (Int32)this._charSize.Height;
            NativeMethods.CreateCaret(this.Handle, IntPtr.Zero, caretWidth, caretHeight);

            this.UpdateCaret();

            NativeMethods.ShowCaret(this.Handle);

            this._caretVisible = true;
        }

        void UpdateCaret()
        {
            if (this._byteProvider == null || this._keyInterpreter == null)
                return;

            System.Diagnostics.Debug.WriteLine("UpdateCaret()", "HexBox");

            Int64 byteIndex = this._bytePos - this._startByte;
            PointF p = this._keyInterpreter.GetCaretPointF(byteIndex);
            p.X += this._byteCharacterPos * this._charSize.Width;
            NativeMethods.SetCaretPos((Int32)p.X, (Int32)p.Y);
        }

        void DestroyCaret()
        {
            if (!this._caretVisible)
                return;

            System.Diagnostics.Debug.WriteLine("DestroyCaret()", "HexBox");

            NativeMethods.DestroyCaret();
            this._caretVisible = false;
        }

        void SetCaretPosition(Point p)
        {
            System.Diagnostics.Debug.WriteLine("SetCaretPosition()", "HexBox");

            if (this._byteProvider == null || this._keyInterpreter == null)
                return;

            Int64 pos = this._bytePos;
            Int32 cp = this._byteCharacterPos;

            if (this._recHex.Contains(p))
            {
                BytePositionInfo bpi = this.GetHexBytePositionInfo(p);
                pos = bpi.Index;
                cp = bpi.CharacterPosition;

                this.SetPosition(pos, cp);

                this.ActivateKeyInterpreter();
                this.UpdateCaret();
                this.Invalidate();
            }
            else if (this._recStringView.Contains(p))
            {
                BytePositionInfo bpi = this.GetStringBytePositionInfo(p);
                pos = bpi.Index;
                cp = bpi.CharacterPosition;

                this.SetPosition(pos, cp);

                this.ActivateStringKeyInterpreter();
                this.UpdateCaret();
                this.Invalidate();
            }
        }

        BytePositionInfo GetHexBytePositionInfo(Point p)
        {
            System.Diagnostics.Debug.WriteLine("GetHexBytePositionInfo()", "HexBox");

            Int64 bytePos;
            Int32 byteCharaterPos;

            Single x = ((Single)(p.X - this._recHex.X) / this._charSize.Width);
            Single y = ((Single)(p.Y - this._recHex.Y) / this._charSize.Height);
            Int32 iX = (Int32)x;
            Int32 iY = (Int32)y;

            Int32 hPos = (iX / 3 + 1);

            bytePos = Math.Min(this._byteProvider.Length,
                this._startByte + (this._iHexMaxHBytes * (iY + 1) - this._iHexMaxHBytes) + hPos - 1);
            byteCharaterPos = (iX % 3);
            if (byteCharaterPos > 1)
                byteCharaterPos = 1;

            if (bytePos == this._byteProvider.Length)
                byteCharaterPos = 0;

            if (bytePos < 0)
                return new BytePositionInfo(0, 0);
            return new BytePositionInfo(bytePos, byteCharaterPos);
        }

        BytePositionInfo GetStringBytePositionInfo(Point p)
        {
            System.Diagnostics.Debug.WriteLine("GetStringBytePositionInfo()", "HexBox");

            Int64 bytePos;
            Int32 byteCharacterPos;

            Single x = ((Single)(p.X - this._recStringView.X) / this._charSize.Width);
            Single y = ((Single)(p.Y - this._recStringView.Y) / this._charSize.Height);
            Int32 iX = (Int32)x;
            Int32 iY = (Int32)y;

            Int32 hPos = iX + 1;

            bytePos = Math.Min(this._byteProvider.Length,
                this._startByte + (this._iHexMaxHBytes * (iY + 1) - this._iHexMaxHBytes) + hPos - 1);
            byteCharacterPos = 0;

            if (bytePos < 0)
                return new BytePositionInfo(0, 0);
            return new BytePositionInfo(bytePos, byteCharacterPos);
        }
        #endregion

        #region Scroll methods
        void _vScrollBar_Scroll(Object sender, ScrollEventArgs e)
		{
			switch (e.Type)
			{
				case ScrollEventType.Last:
					break;
				case ScrollEventType.EndScroll:
					break;
				case ScrollEventType.SmallIncrement:
                    this.PerformScrollLineDown();
					break;
				case ScrollEventType.SmallDecrement:
                    this.PerformScrollLineUp();
					break;
				case ScrollEventType.LargeIncrement:
                    this.PerformScrollPageDown();
					break;
				case ScrollEventType.LargeDecrement:
                    this.PerformScrollPageUp();
					break;
				case ScrollEventType.ThumbPosition:
                    Int64 lPos = this.FromScrollPos(e.NewValue);
                    this.PerformScrollThumpPosition(lPos);
					break;
				case ScrollEventType.ThumbTrack:
					// to avoid performance problems use a refresh delay implemented with a timer
					if (this._thumbTrackTimer.Enabled) // stop old timer
                        this._thumbTrackTimer.Enabled = false;

                    // perform scroll immediately only if last refresh is very old
                    Int32 currentThumbTrack = System.Environment.TickCount;
					if (currentThumbTrack - this._lastThumbtrack > THUMPTRACKDELAY)
					{
                        this.PerformScrollThumbTrack(null, null);
                        this._lastThumbtrack = currentThumbTrack;
						break;
					}

                    // start thumbtrack timer 
                    this._thumbTrackPosition = this.FromScrollPos(e.NewValue);
                    this._thumbTrackTimer.Enabled = true;
					break;
				case ScrollEventType.First:
					break;
				default:
					break;
			}

			e.NewValue = this.ToScrollPos(this._scrollVpos);
		}

		/// <summary>
		/// Performs the thumbtrack scrolling after an delay.
		/// </summary>
		void PerformScrollThumbTrack(Object sender, EventArgs e)
		{
            this._thumbTrackTimer.Enabled = false;
            this.PerformScrollThumpPosition(this._thumbTrackPosition);
            this._lastThumbtrack = Environment.TickCount;
		}

		void UpdateScrollSize()
		{
			System.Diagnostics.Debug.WriteLine("UpdateScrollSize()", "HexBox");

			// calc scroll bar info
			if (this.VScrollBarVisible && this._byteProvider != null && this._byteProvider.Length > 0 && this._iHexMaxHBytes != 0)
			{
                Int64 scrollmax = (Int64)Math.Ceiling((Double)(this._byteProvider.Length + 1) / (Double)this._iHexMaxHBytes - (Double)this._iHexMaxVBytes);
				scrollmax = Math.Max(0, scrollmax);

                Int64 scrollpos = this._startByte / this._iHexMaxHBytes;

				if (scrollmax < this._scrollVmax)
				{
					/* Data size has been decreased. */
					if (this._scrollVpos == this._scrollVmax)
                        /* Scroll one line up if we at bottom. */
                        this.PerformScrollLineUp();
				}

				if (scrollmax == this._scrollVmax && scrollpos == this._scrollVpos)
					return;

                this._scrollVmin = 0;
                this._scrollVmax = scrollmax;
                this._scrollVpos = Math.Min(scrollpos, scrollmax);
                this.UpdateVScroll();
			}
			else if (this.VScrollBarVisible)
			{
                // disable scroll bar
                this._scrollVmin = 0;
                this._scrollVmax = 0;
                this._scrollVpos = 0;
                this.UpdateVScroll();
			}
		}

		void UpdateVScroll()
		{
			System.Diagnostics.Debug.WriteLine("UpdateVScroll()", "HexBox");

            Int32 max = this.ToScrollMax(this._scrollVmax);

			if (max > 0)
			{
                this._vScrollBar.Minimum = 0;
                this._vScrollBar.Maximum = max;
                this._vScrollBar.Value = this.ToScrollPos(this._scrollVpos);
                this._vScrollBar.Visible = true;
			}
			else
			{
                this._vScrollBar.Visible = false;
			}
		}

        Int32 ToScrollPos(Int64 value)
		{
            Int32 max = 65535;

			if (this._scrollVmax < max)
				return (Int32)value;
			else
			{
                Double valperc = (Double)value / (Double)this._scrollVmax * (Double)100;
                Int32 res = (Int32)Math.Floor((Double)max / (Double)100 * valperc);
				res = (Int32)Math.Max(this._scrollVmin, res);
				res = (Int32)Math.Min(this._scrollVmax, res);
				return res;
			}
		}

        Int64 FromScrollPos(Int32 value)
		{
            Int32 max = 65535;
			if (this._scrollVmax < max)
			{
				return (Int64)value;
			}
			else
			{
                Double valperc = (Double)value / (Double)max * (Double)100;
                Int64 res = (Int32)Math.Floor((Double)this._scrollVmax / (Double)100 * valperc);
				return res;
			}
		}

        Int32 ToScrollMax(Int64 value)
		{
            Int64 max = 65535;
			if (value > max)
				return (Int32)max;
			else
				return (Int32)value;
		}

		void PerformScrollToLine(Int64 pos)
		{
			if (pos < this._scrollVmin || pos > this._scrollVmax || pos == this._scrollVpos)
				return;

            this._scrollVpos = pos;

            this.UpdateVScroll();
            this.UpdateVisibilityBytes();
            this.UpdateCaret();
            this.Invalidate();
		}

		void PerformScrollLines(Int32 lines)
		{
            Int64 pos;
			if (lines > 0)
			{
				pos = Math.Min(this._scrollVmax, this._scrollVpos + lines);
			}
			else if (lines < 0)
			{
				pos = Math.Max(this._scrollVmin, this._scrollVpos + lines);
			}
			else
			{
				return;
			}

            this.PerformScrollToLine(pos);
		}

		void PerformScrollLineDown()
		{
			this.PerformScrollLines(1);
		}

		void PerformScrollLineUp()
		{
			this.PerformScrollLines(-1);
		}

		void PerformScrollPageDown()
		{
			this.PerformScrollLines(this._iHexMaxVBytes);
		}

		void PerformScrollPageUp()
		{
			this.PerformScrollLines(-this._iHexMaxVBytes);
		}

		void PerformScrollThumpPosition(Int64 pos)
		{
            // Bug fix: Scroll to end, do not scroll to end
            Int32 difference = (this._scrollVmax > 65535) ? 10 : 9;

			if (this.ToScrollPos(pos) == this.ToScrollMax(this._scrollVmax) - difference)
				pos = this._scrollVmax;
            // End Bug fix


            this.PerformScrollToLine(pos);
		}

		/// <summary>
		/// Scrolls the selection start byte into view
		/// </summary>
		public void ScrollByteIntoView()
		{
			System.Diagnostics.Debug.WriteLine("ScrollByteIntoView()", "HexBox");

            this.ScrollByteIntoView(this._bytePos);
		}

		/// <summary>
		/// Scrolls the specific byte into view
		/// </summary>
		/// <param name="index">the index of the byte</param>
		public void ScrollByteIntoView(Int64 index)
		{
			System.Diagnostics.Debug.WriteLine("ScrollByteIntoView(long index)", "HexBox");

			if (this._byteProvider == null || this._keyInterpreter == null)
				return;

			if (index < this._startByte)
			{
                Int64 line = (Int64)Math.Floor((Double)index / (Double)this._iHexMaxHBytes);
                this.PerformScrollThumpPosition(line);
			}
			else if (index > this._endByte)
			{
                Int64 line = (Int64)Math.Floor((Double)index / (Double)this._iHexMaxHBytes);
				line -= this._iHexMaxVBytes - 1;
                this.PerformScrollThumpPosition(line);
			}
		}
		#endregion

		#region Selection methods
		void ReleaseSelection()
		{
			System.Diagnostics.Debug.WriteLine("ReleaseSelection()", "HexBox");

			if (this._selectionLength == 0)
				return;
            this._selectionLength = 0;
            this.OnSelectionLengthChanged(EventArgs.Empty);

			if (!this._caretVisible)
                this.CreateCaret();
			else
                this.UpdateCaret();

            this.Invalidate();
		}

		/// <summary>
		/// Returns true if Select method could be invoked.
		/// </summary>
		public Boolean CanSelectAll()
		{
			if (!this.Enabled)
				return false;
			if (this._byteProvider == null)
				return false;

			return true;
		}

		/// <summary>
		/// Selects all bytes.
		/// </summary>
		public void SelectAll()
		{
			if (this.ByteProvider == null)
				return;
			this.Select(0, this.ByteProvider.Length);
		}

		/// <summary>
		/// Selects the hex box.
		/// </summary>
		/// <param name="start">the start index of the selection</param>
		/// <param name="length">the length of the selection</param>
		public void Select(Int64 start, Int64 length)
		{
			if (this.ByteProvider == null)
				return;
			if (!this.Enabled)
				return;

            this.InternalSelect(start, length);
            this.ScrollByteIntoView();
		}

		void InternalSelect(Int64 start, Int64 length)
		{
            Int64 pos = start;
            Int64 sel = length;
            Int32 cp = 0;

			if (sel > 0 && this._caretVisible)
                this.DestroyCaret();
			else if (sel == 0 && !this._caretVisible)
                this.CreateCaret();

            this.SetPosition(pos, cp);
            this.SetSelectionLength(sel);

            this.UpdateCaret();
            this.Invalidate();
		}
        #endregion

        #region Find methods
        /// <summary>
        /// Searches the current ByteProvider
        /// </summary>
        /// <param name="options">contains all find options</param>
        /// <returns>the SelectionStart property value if find was successfull or
        /// -1 if there is no match
        /// -2 if Find was aborted.</returns>
        public Int64 Find(Byte[] findHex)
        {
            if (findHex == null || findHex.Length == 0)
                throw new ArgumentException("Hex can not be null when Type is Hex");

            return this.Search(findHex, null);
        }
        public Int64 Find(String findText, Boolean matchCase)
        {
            Byte[] findBuffer = ASCIIEncoding.ASCII.GetBytes(findText);
            Byte[] findBufferLowerCase = ASCIIEncoding.ASCII.GetBytes(findText.ToLower());
            Byte[] findBufferUpperCase = ASCIIEncoding.ASCII.GetBytes(findText.ToUpper());

            if (matchCase)
            {
                if (findBuffer == null || findBuffer.Length == 0)
                    throw new ArgumentException("FindBuffer can not be null when Type: Text and MatchCase: true");

                return this.Search(findBuffer, null);
            }
            else
            {
                if (findBufferLowerCase == null || findBufferLowerCase.Length == 0)
                    throw new ArgumentException("FindBufferLowerCase can not be null when Type is Text and MatchCase is false");
                if (findBufferUpperCase == null || findBufferUpperCase.Length == 0)
                    throw new ArgumentException("FindBufferUpperCase can not be null when Type is Text and MatchCase is false");
                if (findBufferLowerCase.Length != findBufferUpperCase.Length)
                    throw new ArgumentException("FindBufferUpperCase and FindBufferUpperCase must have the same size when Type is Text and MatchCase is true");

                return this.Search(findBufferLowerCase, findBufferUpperCase);
            }
        }
        Int64 Search(Byte[] buffer1, Byte[] buffer2)
        {
            var startIndex = this.SelectionStart + this.SelectionLength;
            Int32 match = 0;

            Int32 buffer1Length = buffer1.Length;

            this._abortFind = false;

            for (Int64 pos = startIndex; pos < this._byteProvider.Length; pos++)
            {
                if (this._abortFind)
                    return -2;

                if (pos % 100 == 0) // for performance reasons: DoEvents only 1 times per 100 loops
                    Application.DoEvents();

                Byte compareByte = this._byteProvider.ReadByte(pos);
                Boolean buffer1Match = compareByte == buffer1[match];
                Boolean hasBuffer2 = buffer2 != null;
                Boolean buffer2Match = hasBuffer2 ? compareByte == buffer2[match] : false;
                Boolean isMatch = buffer1Match || buffer2Match;
                if (!isMatch)
                {
                    pos -= match;
                    match = 0;
                    this._findingPos = pos;
                    continue;
                }

                match++;

                if (match == buffer1Length)
                {
                    Int64 bytePos = pos - buffer1Length + 1;
                    this.Select(bytePos, buffer1Length);
                    this.ScrollByteIntoView(this._bytePos + this._selectionLength);
                    this.ScrollByteIntoView(this._bytePos);

                    return bytePos;
                }
            }

            return -1;
        }

        /// <summary>
        /// Aborts a working Find method.
        /// </summary>
        public void AbortFind()
        {
            this._abortFind = true;
        }

        /// <summary>
        /// Gets a value that indicates the current position during Find method execution.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Int64 CurrentFindingPosition
        {
            get
            {
                return this._findingPos;
            }
        }
        #endregion

        #region Copy, Cut and Paste methods
        Byte[] GetCopyData()
        {
            if (!this.CanCopy()) return new Byte[0];

            // put bytes into buffer
            Byte[] buffer = new Byte[this._selectionLength];
            Int32 id = -1;
            for (Int64 i = this._bytePos; i < this._bytePos + this._selectionLength; i++)
            {
                id++;

                buffer[id] = this._byteProvider.ReadByte(i);
            }
            return buffer;
        }
        /// <summary>
        /// Copies the current selection in the hex box to the Clipboard.
        /// </summary>
        public void Copy()
        {
            if (!this.CanCopy()) return;

            // put bytes into buffer
            Byte[] buffer = this.GetCopyData();

            DataObject da = new DataObject();

            // set string buffer clipbard data
            String sBuffer = System.Text.Encoding.ASCII.GetString(buffer, 0, buffer.Length);
            da.SetData(typeof(String), sBuffer);

            //set memorystream (BinaryData) clipboard data
            System.IO.MemoryStream ms = new System.IO.MemoryStream(buffer, 0, buffer.Length, false, true);
            da.SetData("BinaryData", ms);

            Clipboard.SetDataObject(da, true);
            this.UpdateCaret();
            this.ScrollByteIntoView();
            this.Invalidate();

            this.OnCopied(EventArgs.Empty);
        }

        /// <summary>
        /// Return true if Copy method could be invoked.
        /// </summary>
        public Boolean CanCopy()
        {
            if (this._selectionLength < 1 || this._byteProvider == null)
                return false;

            return true;
        }

        /// <summary>
        /// Moves the current selection in the hex box to the Clipboard.
        /// </summary>
        public void Cut()
        {
            if (!this.CanCut()) return;

            this.Copy();

            this._byteProvider.DeleteBytes(this._bytePos, this._selectionLength);
            this._byteCharacterPos = 0;
            this.UpdateCaret();
            this.ScrollByteIntoView();
            this.ReleaseSelection();
            this.Invalidate();
            this.Refresh();
        }

        /// <summary>
        /// Return true if Cut method could be invoked.
        /// </summary>
        public Boolean CanCut()
        {
            if (this.ReadOnly || !this.Enabled)
                return false;
            if (this._byteProvider == null)
                return false;
            if (this._selectionLength < 1 || !this._byteProvider.SupportsDeleteBytes())
                return false;

            return true;
        }

        /// <summary>
        /// Replaces the current selection in the hex box with the contents of the Clipboard.
        /// </summary>
        public void Paste()
        {
            if (!this.CanPaste()) return;

            if (this._selectionLength > 0)
                this._byteProvider.DeleteBytes(this._bytePos, this._selectionLength);

            Byte[] data = null;
            IDataObject paste = Clipboard.GetDataObject();
            if (paste.GetDataPresent("BinaryData"))
            {
                System.IO.MemoryStream ms = (System.IO.MemoryStream)paste.GetData("BinaryData");
                data = new Byte[ms.Length];
                ms.Read(data, 0, data.Length);
            }
            else if (paste.GetDataPresent(typeof(String)))
            {
                String sBuffer = (String)paste.GetData(typeof(String));
                try
                {
                    data = Util.SpacedHexToBytes(sBuffer.Trim());
                }
                catch
                {
                    data = Encoding.ASCII.GetBytes(sBuffer);
                }
            }
            else
            {
                return;
            }

            this._byteProvider.InsertBytes(this._bytePos, data);

            this.SetPosition(this._bytePos + data.Length, 0);

            this.ReleaseSelection();
            this.ScrollByteIntoView();
            this.UpdateCaret();
            this.Invalidate();
        }

        /// <summary>
        /// Return true if Paste method could be invoked.
        /// </summary>
        public Boolean CanPaste()
        {
            if (this.ReadOnly || !this.Enabled) return false;

            if (this._byteProvider == null || !this._byteProvider.SupportsInsertBytes())
                return false;

            if (!this._byteProvider.SupportsDeleteBytes() && this._selectionLength > 0)
                return false;

            IDataObject da = Clipboard.GetDataObject();
            if (da.GetDataPresent("BinaryData"))
                return true;
            else if (da.GetDataPresent(typeof(String)))
                return true;
            else
                return false;
        }
        /// <summary>
        /// Return true if PasteHex method could be invoked.
        /// </summary>
        public Boolean CanPasteHex()
        {
            if (!this.CanPaste()) return false;

            Byte[] buffer = null;
            IDataObject da = Clipboard.GetDataObject();
            if (da.GetDataPresent(typeof(String)))
            {
                String hexString = (String)da.GetData(typeof(String));
                buffer = this.ConvertHexToBytes(hexString);
                return (buffer != null);
            }
            return false;
        }

        /// <summary>
        /// Replaces the current selection in the hex box with the hex string data of the Clipboard.
        /// </summary>
        public void PasteHex()
        {
            if (!this.CanPaste()) return;

            Byte[] buffer = null;
            IDataObject da = Clipboard.GetDataObject();
            if (da.GetDataPresent(typeof(String)))
            {
                String hexString = (String)da.GetData(typeof(String));
                buffer = this.ConvertHexToBytes(hexString);
                if (buffer == null)
                    return;
            }
            else
            {
                return;
            }

            if (this._selectionLength > 0)
                this._byteProvider.DeleteBytes(this._bytePos, this._selectionLength);

            this._byteProvider.InsertBytes(this._bytePos, buffer);

            this.SetPosition(this._bytePos + buffer.Length, 0);

            this.ReleaseSelection();
            this.ScrollByteIntoView();
            this.UpdateCaret();
            this.Invalidate();
        }

        /// <summary>
        /// Copies the current selection in the hex box to the Clipboard in hex format.
        /// </summary>
        public void CopyHex()
        {
            if (!this.CanCopy()) return;

            // put bytes into buffer
            Byte[] buffer = this.GetCopyData();

            DataObject da = new DataObject();

            // set string buffer clipbard data
            String hexString = this.ConvertBytesToHex(buffer); ;
            da.SetData(typeof(String), hexString);

            //set memorystream (BinaryData) clipboard data
            System.IO.MemoryStream ms = new System.IO.MemoryStream(buffer, 0, buffer.Length, false, true);
            da.SetData("BinaryData", ms);

            Clipboard.SetDataObject(da, true);
            this.UpdateCaret();
            this.ScrollByteIntoView();
            this.Invalidate();

            this.OnCopiedHex(EventArgs.Empty);
        }


        #endregion

        #region PreProcessMessage methods
        /// <summary>
        /// Preprocesses windows messages.
        /// </summary>
        /// <param name="m">the message to process.</param>
        /// <returns>true, if the message was processed</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true), SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
        public override Boolean PreProcessMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case NativeMethods.WM_KEYDOWN:
                    return this._keyInterpreter.PreProcessWmKeyDown(ref m);
                case NativeMethods.WM_CHAR:
                    return this._keyInterpreter.PreProcessWmChar(ref m);
                case NativeMethods.WM_KEYUP:
                    return this._keyInterpreter.PreProcessWmKeyUp(ref m);
                default:
                    return base.PreProcessMessage(ref m);
            }
        }

        Boolean BasePreProcessMessage(ref Message m)
        {
            return base.PreProcessMessage(ref m);
        }
        #endregion

        #region Key interpreter methods
        void ActivateEmptyKeyInterpreter()
		{
			if (this._eki == null)
                this._eki = new EmptyKeyInterpreter(this);

			if (this._eki == this._keyInterpreter)
				return;

			if (this._keyInterpreter != null)
                this._keyInterpreter.Deactivate();

            this._keyInterpreter = this._eki;
            this._keyInterpreter.Activate();
		}

		void ActivateKeyInterpreter()
		{
			if (this._ki == null)
                this._ki = new KeyInterpreter(this);

			if (this._ki == this._keyInterpreter)
				return;

			if (this._keyInterpreter != null)
                this._keyInterpreter.Deactivate();

            this._keyInterpreter = this._ki;
            this._keyInterpreter.Activate();
		}

		void ActivateStringKeyInterpreter()
		{
			if (this._ski == null)
                this._ski = new StringKeyInterpreter(this);

			if (this._ski == this._keyInterpreter)
				return;

			if (this._keyInterpreter != null)
                this._keyInterpreter.Deactivate();

            this._keyInterpreter = this._ski;
            this._keyInterpreter.Activate();
		}
        #endregion

        #region IKeyInterpreter interface
        /// <summary>
        /// Defines a user input handler such as for mouse and keyboard input
        /// </summary>
        interface IKeyInterpreter
        {
            /// <summary>
            /// Activates mouse events
            /// </summary>
            void Activate();
            /// <summary>
            /// Deactivate mouse events
            /// </summary>
            void Deactivate();
            /// <summary>
            /// Preprocesses WM_KEYUP window message.
            /// </summary>
            /// <param name="m">the Message object to process.</param>
            /// <returns>True, if the message was processed.</returns>
            Boolean PreProcessWmKeyUp(ref Message m);
            /// <summary>
            /// Preprocesses WM_CHAR window message.
            /// </summary>
            /// <param name="m">the Message object to process.</param>
            /// <returns>True, if the message was processed.</returns>
            Boolean PreProcessWmChar(ref Message m);
            /// <summary>
            /// Preprocesses WM_KEYDOWN window message.
            /// </summary>
            /// <param name="m">the Message object to process.</param>
            /// <returns>True, if the message was processed.</returns>
            Boolean PreProcessWmKeyDown(ref Message m);
            /// <summary>
            /// Gives some information about where to place the caret.
            /// </summary>
            /// <param name="byteIndex">the index of the byte</param>
            /// <returns>the position where the caret is to place.</returns>
            PointF GetCaretPointF(Int64 byteIndex);
        }
        #endregion
        #region EmptyKeyInterpreter class
        /// <summary>
        /// Represents an empty input handler without any functionality. 
        /// If is set ByteProvider to null, then this interpreter is used.
        /// </summary>
        class EmptyKeyInterpreter : IKeyInterpreter
        {
            HexBox _hexBox;

            public EmptyKeyInterpreter(HexBox hexBox)
            {
                this._hexBox = hexBox;
            }

            #region IKeyInterpreter Members
            public void Activate() { }
            public void Deactivate() { }

            public Boolean PreProcessWmKeyUp(ref Message m)
            { return this._hexBox.BasePreProcessMessage(ref m); }

            public Boolean PreProcessWmChar(ref Message m)
            { return this._hexBox.BasePreProcessMessage(ref m); }

            public Boolean PreProcessWmKeyDown(ref Message m)
            { return this._hexBox.BasePreProcessMessage(ref m); }

            public PointF GetCaretPointF(Int64 byteIndex)
            { return new PointF(); }

            #endregion
        }
        #endregion
        #region KeyInterpreter class
        /// <summary>
        /// Handles user input such as mouse and keyboard input during hex view edit
        /// </summary>
        class KeyInterpreter : IKeyInterpreter
        {
            /// <summary>
            /// Delegate for key-down processing.
            /// </summary>
            /// <param name="m">the message object contains key data information</param>
            /// <returns>True, if the message was processed</returns>
            delegate Boolean MessageDelegate(ref Message m);

            #region Fields
            /// <summary>
            /// Contains the parent HexBox control
            /// </summary>
            protected HexBox _hexBox;

            /// <summary>
            /// Contains True, if shift key is down
            /// </summary>
            protected Boolean _shiftDown;
            /// <summary>
            /// Contains True, if mouse is down
            /// </summary>
            Boolean _mouseDown;
            /// <summary>
            /// Contains the selection start position info
            /// </summary>
            BytePositionInfo _bpiStart;
            /// <summary>
            /// Contains the current mouse selection position info
            /// </summary>
            BytePositionInfo _bpi;
            /// <summary>
            /// Contains all message handlers of key interpreter key down message
            /// </summary>
            Dictionary<Keys, MessageDelegate> _messageHandlers;
            #endregion

            #region Ctors
            public KeyInterpreter(HexBox hexBox)
            {
                this._hexBox = hexBox;
            }
            #endregion

            #region Activate, Deactive methods
            public virtual void Activate()
            {
                this._hexBox.MouseDown += new MouseEventHandler(this.BeginMouseSelection);
                this._hexBox.MouseMove += new MouseEventHandler(this.UpdateMouseSelection);
                this._hexBox.MouseUp += new MouseEventHandler(this.EndMouseSelection);
            }

            public virtual void Deactivate()
            {
                this._hexBox.MouseDown -= new MouseEventHandler(this.BeginMouseSelection);
                this._hexBox.MouseMove -= new MouseEventHandler(this.UpdateMouseSelection);
                this._hexBox.MouseUp -= new MouseEventHandler(this.EndMouseSelection);
            }
            #endregion

            #region Mouse selection methods
            void BeginMouseSelection(Object sender, MouseEventArgs e)
            {
                System.Diagnostics.Debug.WriteLine("BeginMouseSelection()", "KeyInterpreter");

                if (e.Button != MouseButtons.Left)
                    return;

                this._mouseDown = true;

                if (!this._shiftDown)
                {
                    this._bpiStart = new BytePositionInfo(this._hexBox._bytePos, this._hexBox._byteCharacterPos);
                    this._hexBox.ReleaseSelection();
                }
                else
                {
                    this.UpdateMouseSelection(this, e);
                }
            }

            void UpdateMouseSelection(Object sender, MouseEventArgs e)
            {
                if (!this._mouseDown)
                    return;

                this._bpi = this.GetBytePositionInfo(new Point(e.X, e.Y));
                Int64 selEnd = this._bpi.Index;
                Int64 realselStart;
                Int64 realselLength;

                if (selEnd < this._bpiStart.Index)
                {
                    realselStart = selEnd;
                    realselLength = this._bpiStart.Index - selEnd;
                }
                else if (selEnd > this._bpiStart.Index)
                {
                    realselStart = this._bpiStart.Index;
                    realselLength = selEnd - realselStart;
                }
                else
                {
                    realselStart = this._hexBox._bytePos;
                    realselLength = 0;
                }

                if (realselStart != this._hexBox._bytePos || realselLength != this._hexBox._selectionLength)
                {
                    this._hexBox.InternalSelect(realselStart, realselLength);
                    this._hexBox.ScrollByteIntoView(this._bpi.Index);
                }
            }

            void EndMouseSelection(Object sender, MouseEventArgs e)
            {
                this._mouseDown = false;
            }
            #endregion

            #region PrePrcessWmKeyDown methods
            public virtual Boolean PreProcessWmKeyDown(ref Message m)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessWmKeyDown(ref Message m)", "KeyInterpreter");

                Keys vc = (Keys)m.WParam.ToInt32();

                Keys keyData = vc | Control.ModifierKeys;

                // detect whether key down event should be raised
                var hasMessageHandler = this.MessageHandlers.ContainsKey(keyData);
                if (hasMessageHandler && this.RaiseKeyDown(keyData))
                    return true;

                MessageDelegate messageHandler = hasMessageHandler
                    ? this.MessageHandlers[keyData]
                    : messageHandler = new MessageDelegate(this.PreProcessWmKeyDown_Default);

                return messageHandler(ref m);
            }

            protected Boolean PreProcessWmKeyDown_Default(ref Message m)
            {
                this._hexBox.ScrollByteIntoView();
                return this._hexBox.BasePreProcessMessage(ref m);
            }

            protected Boolean RaiseKeyDown(Keys keyData)
            {
                KeyEventArgs e = new KeyEventArgs(keyData);
                this._hexBox.OnKeyDown(e);
                return e.Handled;
            }

            protected virtual Boolean PreProcessWmKeyDown_Left(ref Message m)
            {
                return this.PerformPosMoveLeft();
            }

            protected virtual Boolean PreProcessWmKeyDown_Up(ref Message m)
            {
                Int64 pos = this._hexBox._bytePos;
                Int32 cp = this._hexBox._byteCharacterPos;

                if (!(pos == 0 && cp == 0))
                {
                    pos = Math.Max(-1, pos - this._hexBox._iHexMaxHBytes);
                    if (pos == -1)
                        return true;

                    this._hexBox.SetPosition(pos);

                    if (pos < this._hexBox._startByte)
                    {
                        this._hexBox.PerformScrollLineUp();
                    }

                    this._hexBox.UpdateCaret();
                    this._hexBox.Invalidate();
                }

                this._hexBox.ScrollByteIntoView();
                this._hexBox.ReleaseSelection();

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_Right(ref Message m)
            {
                return this.PerformPosMoveRight();
            }

            protected virtual Boolean PreProcessWmKeyDown_Down(ref Message m)
            {
                Int64 pos = this._hexBox._bytePos;
                Int32 cp = this._hexBox._byteCharacterPos;

                if (pos == this._hexBox._byteProvider.Length && cp == 0)
                    return true;

                pos = Math.Min(this._hexBox._byteProvider.Length, pos + this._hexBox._iHexMaxHBytes);

                if (pos == this._hexBox._byteProvider.Length)
                    cp = 0;

                this._hexBox.SetPosition(pos, cp);

                if (pos > this._hexBox._endByte - 1)
                {
                    this._hexBox.PerformScrollLineDown();
                }

                this._hexBox.UpdateCaret();
                this._hexBox.ScrollByteIntoView();
                this._hexBox.ReleaseSelection();
                this._hexBox.Invalidate();

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_PageUp(ref Message m)
            {
                Int64 pos = this._hexBox._bytePos;
                Int32 cp = this._hexBox._byteCharacterPos;

                if (pos == 0 && cp == 0)
                    return true;

                pos = Math.Max(0, pos - this._hexBox._iHexMaxBytes);
                if (pos == 0)
                    return true;

                this._hexBox.SetPosition(pos);

                if (pos < this._hexBox._startByte)
                {
                    this._hexBox.PerformScrollPageUp();
                }

                this._hexBox.ReleaseSelection();
                this._hexBox.UpdateCaret();
                this._hexBox.Invalidate();
                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_PageDown(ref Message m)
            {
                Int64 pos = this._hexBox._bytePos;
                Int32 cp = this._hexBox._byteCharacterPos;

                if (pos == this._hexBox._byteProvider.Length && cp == 0)
                    return true;

                pos = Math.Min(this._hexBox._byteProvider.Length, pos + this._hexBox._iHexMaxBytes);

                if (pos == this._hexBox._byteProvider.Length)
                    cp = 0;

                this._hexBox.SetPosition(pos, cp);

                if (pos > this._hexBox._endByte - 1)
                {
                    this._hexBox.PerformScrollPageDown();
                }

                this._hexBox.ReleaseSelection();
                this._hexBox.UpdateCaret();
                this._hexBox.Invalidate();

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_ShiftLeft(ref Message m)
            {
                Int64 pos = this._hexBox._bytePos;
                Int64 sel = this._hexBox._selectionLength;

                if (pos + sel < 1)
                    return true;

                if (pos + sel <= this._bpiStart.Index)
                {
                    if (pos == 0)
                        return true;

                    pos--;
                    sel++;
                }
                else
                {
                    sel = Math.Max(0, sel - 1);
                }

                this._hexBox.ScrollByteIntoView();
                this._hexBox.InternalSelect(pos, sel);

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_ShiftUp(ref Message m)
            {
                Int64 pos = this._hexBox._bytePos;
                Int64 sel = this._hexBox._selectionLength;

                if (pos - this._hexBox._iHexMaxHBytes < 0 && pos <= this._bpiStart.Index)
                    return true;

                if (this._bpiStart.Index >= pos + sel)
                {
                    pos = pos - this._hexBox._iHexMaxHBytes;
                    sel += this._hexBox._iHexMaxHBytes;
                    this._hexBox.InternalSelect(pos, sel);
                    this._hexBox.ScrollByteIntoView();
                }
                else
                {
                    sel -= this._hexBox._iHexMaxHBytes;
                    if (sel < 0)
                    {
                        pos = this._bpiStart.Index + sel;
                        sel = -sel;
                        this._hexBox.InternalSelect(pos, sel);
                        this._hexBox.ScrollByteIntoView();
                    }
                    else
                    {
                        sel -= this._hexBox._iHexMaxHBytes;
                        this._hexBox.InternalSelect(pos, sel);
                        this._hexBox.ScrollByteIntoView(pos + sel);
                    }
                }

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_ShiftRight(ref Message m)
            {
                Int64 pos = this._hexBox._bytePos;
                Int64 sel = this._hexBox._selectionLength;

                if (pos + sel >= this._hexBox._byteProvider.Length)
                    return true;

                if (this._bpiStart.Index <= pos)
                {
                    sel++;
                    this._hexBox.InternalSelect(pos, sel);
                    this._hexBox.ScrollByteIntoView(pos + sel);
                }
                else
                {
                    pos++;
                    sel = Math.Max(0, sel - 1);
                    this._hexBox.InternalSelect(pos, sel);
                    this._hexBox.ScrollByteIntoView();
                }

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_ShiftDown(ref Message m)
            {
                Int64 pos = this._hexBox._bytePos;
                Int64 sel = this._hexBox._selectionLength;

                Int64 max = this._hexBox._byteProvider.Length;

                if (pos + sel + this._hexBox._iHexMaxHBytes > max)
                    return true;

                if (this._bpiStart.Index <= pos)
                {
                    sel += this._hexBox._iHexMaxHBytes;
                    this._hexBox.InternalSelect(pos, sel);
                    this._hexBox.ScrollByteIntoView(pos + sel);
                }
                else
                {
                    sel -= this._hexBox._iHexMaxHBytes;
                    if (sel < 0)
                    {
                        pos = this._bpiStart.Index;
                        sel = -sel;
                    }
                    else
                    {
                        pos += this._hexBox._iHexMaxHBytes;
                        //sel -= _hexBox._iHexMaxHBytes;
                    }

                    this._hexBox.InternalSelect(pos, sel);
                    this._hexBox.ScrollByteIntoView();
                }

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_Tab(ref Message m)
            {
                if (this._hexBox._stringViewVisible && this._hexBox._keyInterpreter.GetType() == typeof(KeyInterpreter))
                {
                    this._hexBox.ActivateStringKeyInterpreter();
                    this._hexBox.ScrollByteIntoView();
                    this._hexBox.ReleaseSelection();
                    this._hexBox.UpdateCaret();
                    this._hexBox.Invalidate();
                    return true;
                }

                if (this._hexBox.Parent == null) return true;
                this._hexBox.Parent.SelectNextControl(this._hexBox, true, true, true, true);
                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_ShiftTab(ref Message m)
            {
                if (this._hexBox._keyInterpreter is StringKeyInterpreter)
                {
                    this._shiftDown = false;
                    this._hexBox.ActivateKeyInterpreter();
                    this._hexBox.ScrollByteIntoView();
                    this._hexBox.ReleaseSelection();
                    this._hexBox.UpdateCaret();
                    this._hexBox.Invalidate();
                    return true;
                }

                if (this._hexBox.Parent == null) return true;
                this._hexBox.Parent.SelectNextControl(this._hexBox, false, true, true, true);
                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_Back(ref Message m)
            {
                if (!this._hexBox._byteProvider.SupportsDeleteBytes())
                    return true;

                if (this._hexBox.ReadOnly)
                    return true;

                Int64 pos = this._hexBox._bytePos;
                Int64 sel = this._hexBox._selectionLength;
                Int32 cp = this._hexBox._byteCharacterPos;

                Int64 startDelete = (cp == 0 && sel == 0) ? pos - 1 : pos;
                if (startDelete < 0 && sel < 1)
                    return true;

                Int64 bytesToDelete = (sel > 0) ? sel : 1;
                this._hexBox._byteProvider.DeleteBytes(Math.Max(0, startDelete), bytesToDelete);
                this._hexBox.UpdateScrollSize();

                if (sel == 0)
                    this.PerformPosMoveLeftByte();

                this._hexBox.ReleaseSelection();
                this._hexBox.Invalidate();

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_Delete(ref Message m)
            {
                if (!this._hexBox._byteProvider.SupportsDeleteBytes())
                    return true;

                if (this._hexBox.ReadOnly)
                    return true;

                Int64 pos = this._hexBox._bytePos;
                Int64 sel = this._hexBox._selectionLength;

                if (pos >= this._hexBox._byteProvider.Length)
                    return true;

                Int64 bytesToDelete = (sel > 0) ? sel : 1;
                this._hexBox._byteProvider.DeleteBytes(pos, bytesToDelete);

                this._hexBox.UpdateScrollSize();
                this._hexBox.ReleaseSelection();
                this._hexBox.Invalidate();

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_Home(ref Message m)
            {
                Int64 pos = this._hexBox._bytePos;
                Int32 cp = this._hexBox._byteCharacterPos;

                if (pos < 1)
                    return true;

                pos = 0;
                cp = 0;
                this._hexBox.SetPosition(pos, cp);

                this._hexBox.ScrollByteIntoView();
                this._hexBox.UpdateCaret();
                this._hexBox.ReleaseSelection();

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_End(ref Message m)
            {
                Int64 pos = this._hexBox._bytePos;
                Int32 cp = this._hexBox._byteCharacterPos;

                if (pos >= this._hexBox._byteProvider.Length - 1)
                    return true;

                pos = this._hexBox._byteProvider.Length;
                cp = 0;
                this._hexBox.SetPosition(pos, cp);

                this._hexBox.ScrollByteIntoView();
                this._hexBox.UpdateCaret();
                this._hexBox.ReleaseSelection();

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_ShiftShiftKey(ref Message m)
            {
                if (this._mouseDown)
                    return true;
                if (this._shiftDown)
                    return true;

                this._shiftDown = true;

                if (this._hexBox._selectionLength > 0)
                    return true;

                this._bpiStart = new BytePositionInfo(this._hexBox._bytePos, this._hexBox._byteCharacterPos);

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_ControlC(ref Message m)
            {
                this._hexBox.Copy();
                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_ControlX(ref Message m)
            {
                this._hexBox.Cut();
                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_ControlV(ref Message m)
            {
                this._hexBox.Paste();
                return true;
            }

            #endregion

            #region PreProcessWmChar methods
            public virtual Boolean PreProcessWmChar(ref Message m)
            {
                if (Control.ModifierKeys == Keys.Control)
                {
                    return this._hexBox.BasePreProcessMessage(ref m);
                }

                Boolean sw = this._hexBox._byteProvider.SupportsWriteByte();
                Boolean si = this._hexBox._byteProvider.SupportsInsertBytes();
                Boolean sd = this._hexBox._byteProvider.SupportsDeleteBytes();

                Int64 pos = this._hexBox._bytePos;
                Int64 sel = this._hexBox._selectionLength;
                Int32 cp = this._hexBox._byteCharacterPos;

                if (
                    (!sw && pos != this._hexBox._byteProvider.Length) ||
                    (!si && pos == this._hexBox._byteProvider.Length))
                {
                    return this._hexBox.BasePreProcessMessage(ref m);
                }

                Char c = (Char)m.WParam.ToInt32();

                if (Uri.IsHexDigit(c))
                {
                    if (this.RaiseKeyPress(c))
                        return true;

                    if (this._hexBox.ReadOnly)
                        return true;

                    Boolean isInsertMode = (pos == this._hexBox._byteProvider.Length);

                    // do insert when insertActive = true
                    if (!isInsertMode && si && this._hexBox.InsertActive && cp == 0)
                        isInsertMode = true;

                    if (sd && si && sel > 0)
                    {
                        this._hexBox._byteProvider.DeleteBytes(pos, sel);
                        isInsertMode = true;
                        cp = 0;
                        this._hexBox.SetPosition(pos, cp);
                    }

                    this._hexBox.ReleaseSelection();

                    Byte currentByte;
                    if (isInsertMode)
                        currentByte = 0;
                    else
                        currentByte = this._hexBox._byteProvider.ReadByte(pos);

                    String sCb = currentByte.ToString("X", System.Threading.Thread.CurrentThread.CurrentCulture);
                    if (sCb.Length == 1)
                        sCb = "0" + sCb;

                    String sNewCb = c.ToString();
                    if (cp == 0)
                        sNewCb += sCb.Substring(1, 1);
                    else
                        sNewCb = sCb.Substring(0, 1) + sNewCb;
                    Byte newcb = Byte.Parse(sNewCb, System.Globalization.NumberStyles.AllowHexSpecifier, System.Threading.Thread.CurrentThread.CurrentCulture);

                    if (isInsertMode)
                        this._hexBox._byteProvider.InsertBytes(pos, new Byte[] { newcb });
                    else
                        this._hexBox._byteProvider.WriteByte(pos, newcb);

                    this.PerformPosMoveRight();

                    this._hexBox.Invalidate();
                    return true;
                }
                else
                {
                    return this._hexBox.BasePreProcessMessage(ref m);
                }
            }

            protected Boolean RaiseKeyPress(Char keyChar)
            {
                KeyPressEventArgs e = new KeyPressEventArgs(keyChar);
                this._hexBox.OnKeyPress(e);
                return e.Handled;
            }
            #endregion

            #region PreProcessWmKeyUp methods
            public virtual Boolean PreProcessWmKeyUp(ref Message m)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessWmKeyUp(ref Message m)", "KeyInterpreter");

                Keys vc = (Keys)m.WParam.ToInt32();

                Keys keyData = vc | Control.ModifierKeys;

                switch (keyData)
                {
                    case Keys.ShiftKey:
                    case Keys.Insert:
                        if (this.RaiseKeyUp(keyData))
                            return true;
                        break;
                }

                switch (keyData)
                {
                    case Keys.ShiftKey:
                        this._shiftDown = false;
                        return true;
                    case Keys.Insert:
                        return this.PreProcessWmKeyUp_Insert(ref m);
                    default:
                        return this._hexBox.BasePreProcessMessage(ref m);
                }
            }

            protected virtual Boolean PreProcessWmKeyUp_Insert(ref Message m)
            {
                this._hexBox.InsertActive = !this._hexBox.InsertActive;
                return true;
            }

            protected Boolean RaiseKeyUp(Keys keyData)
            {
                KeyEventArgs e = new KeyEventArgs(keyData);
                this._hexBox.OnKeyUp(e);
                return e.Handled;
            }
            #endregion

            #region Misc
            Dictionary<Keys, MessageDelegate> MessageHandlers
            {
                get
                {
                    if (this._messageHandlers == null)
                    {
                        this._messageHandlers = new Dictionary<Keys, MessageDelegate>();
                        this._messageHandlers.Add(Keys.Left, new MessageDelegate(this.PreProcessWmKeyDown_Left)); // move left
                        this._messageHandlers.Add(Keys.Up, new MessageDelegate(this.PreProcessWmKeyDown_Up)); // move up
                        this._messageHandlers.Add(Keys.Right, new MessageDelegate(this.PreProcessWmKeyDown_Right)); // move right
                        this._messageHandlers.Add(Keys.Down, new MessageDelegate(this.PreProcessWmKeyDown_Down)); // move down
                        this._messageHandlers.Add(Keys.PageUp, new MessageDelegate(this.PreProcessWmKeyDown_PageUp)); // move pageup
                        this._messageHandlers.Add(Keys.PageDown, new MessageDelegate(this.PreProcessWmKeyDown_PageDown)); // move page down
                        this._messageHandlers.Add(Keys.Left | Keys.Shift, new MessageDelegate(this.PreProcessWmKeyDown_ShiftLeft)); // move left with selection
                        this._messageHandlers.Add(Keys.Up | Keys.Shift, new MessageDelegate(this.PreProcessWmKeyDown_ShiftUp)); // move up with selection
                        this._messageHandlers.Add(Keys.Right | Keys.Shift, new MessageDelegate(this.PreProcessWmKeyDown_ShiftRight)); // move right with selection
                        this._messageHandlers.Add(Keys.Down | Keys.Shift, new MessageDelegate(this.PreProcessWmKeyDown_ShiftDown)); // move down with selection
                        this._messageHandlers.Add(Keys.Tab, new MessageDelegate(this.PreProcessWmKeyDown_Tab)); // switch to string view
                        this._messageHandlers.Add(Keys.Back, new MessageDelegate(this.PreProcessWmKeyDown_Back)); // back
                        this._messageHandlers.Add(Keys.Delete, new MessageDelegate(this.PreProcessWmKeyDown_Delete)); // delete
                        this._messageHandlers.Add(Keys.Home, new MessageDelegate(this.PreProcessWmKeyDown_Home)); // move to home
                        this._messageHandlers.Add(Keys.End, new MessageDelegate(this.PreProcessWmKeyDown_End)); // move to end
                        this._messageHandlers.Add(Keys.ShiftKey | Keys.Shift, new MessageDelegate(this.PreProcessWmKeyDown_ShiftShiftKey)); // begin selection process
                        this._messageHandlers.Add(Keys.C | Keys.Control, new MessageDelegate(this.PreProcessWmKeyDown_ControlC)); // copy 
                        this._messageHandlers.Add(Keys.X | Keys.Control, new MessageDelegate(this.PreProcessWmKeyDown_ControlX)); // cut
                        this._messageHandlers.Add(Keys.V | Keys.Control, new MessageDelegate(this.PreProcessWmKeyDown_ControlV)); // paste
                    }
                    return this._messageHandlers;
                }
            }

            protected virtual Boolean PerformPosMoveLeft()
            {
                Int64 pos = this._hexBox._bytePos;
                Int64 sel = this._hexBox._selectionLength;
                Int32 cp = this._hexBox._byteCharacterPos;

                if (sel != 0)
                {
                    cp = 0;
                    this._hexBox.SetPosition(pos, cp);
                    this._hexBox.ReleaseSelection();
                }
                else
                {
                    if (pos == 0 && cp == 0)
                        return true;

                    if (cp > 0)
                    {
                        cp--;
                    }
                    else
                    {
                        pos = Math.Max(0, pos - 1);
                        cp++;
                    }

                    this._hexBox.SetPosition(pos, cp);

                    if (pos < this._hexBox._startByte)
                    {
                        this._hexBox.PerformScrollLineUp();
                    }
                    this._hexBox.UpdateCaret();
                    this._hexBox.Invalidate();
                }

                this._hexBox.ScrollByteIntoView();
                return true;
            }
            protected virtual Boolean PerformPosMoveRight()
            {
                Int64 pos = this._hexBox._bytePos;
                Int32 cp = this._hexBox._byteCharacterPos;
                Int64 sel = this._hexBox._selectionLength;

                if (sel != 0)
                {
                    pos += sel;
                    cp = 0;
                    this._hexBox.SetPosition(pos, cp);
                    this._hexBox.ReleaseSelection();
                }
                else
                {
                    if (!(pos == this._hexBox._byteProvider.Length && cp == 0))
                    {

                        if (cp > 0)
                        {
                            pos = Math.Min(this._hexBox._byteProvider.Length, pos + 1);
                            cp = 0;
                        }
                        else
                        {
                            cp++;
                        }

                        this._hexBox.SetPosition(pos, cp);

                        if (pos > this._hexBox._endByte - 1)
                        {
                            this._hexBox.PerformScrollLineDown();
                        }
                        this._hexBox.UpdateCaret();
                        this._hexBox.Invalidate();
                    }
                }

                this._hexBox.ScrollByteIntoView();
                return true;
            }
            protected virtual Boolean PerformPosMoveLeftByte()
            {
                Int64 pos = this._hexBox._bytePos;
                Int32 cp = this._hexBox._byteCharacterPos;

                if (pos == 0)
                    return true;

                pos = Math.Max(0, pos - 1);
                cp = 0;

                this._hexBox.SetPosition(pos, cp);

                if (pos < this._hexBox._startByte)
                {
                    this._hexBox.PerformScrollLineUp();
                }
                this._hexBox.UpdateCaret();
                this._hexBox.ScrollByteIntoView();
                this._hexBox.Invalidate();

                return true;
            }

            protected virtual Boolean PerformPosMoveRightByte()
            {
                Int64 pos = this._hexBox._bytePos;
                Int32 cp = this._hexBox._byteCharacterPos;

                if (pos == this._hexBox._byteProvider.Length)
                    return true;

                pos = Math.Min(this._hexBox._byteProvider.Length, pos + 1);
                cp = 0;

                this._hexBox.SetPosition(pos, cp);

                if (pos > this._hexBox._endByte - 1)
                {
                    this._hexBox.PerformScrollLineDown();
                }
                this._hexBox.UpdateCaret();
                this._hexBox.ScrollByteIntoView();
                this._hexBox.Invalidate();

                return true;
            }


            public virtual PointF GetCaretPointF(Int64 byteIndex)
            {
                System.Diagnostics.Debug.WriteLine("GetCaretPointF()", "KeyInterpreter");

                return this._hexBox.GetBytePointF(byteIndex);
            }

            protected virtual BytePositionInfo GetBytePositionInfo(Point p)
            {
                return this._hexBox.GetHexBytePositionInfo(p);
            }
            #endregion
        }
        #endregion
        #region StringKeyInterpreter class
        /// <summary>
        /// Handles user input such as mouse and keyboard input during string view edit
        /// </summary>
        class StringKeyInterpreter : KeyInterpreter
        {
            #region Ctors
            public StringKeyInterpreter(HexBox hexBox)
                : base(hexBox)
            {
                this._hexBox._byteCharacterPos = 0;
            }
            #endregion

            #region PreProcessWmKeyDown methods
            public override Boolean PreProcessWmKeyDown(ref Message m)
            {
                Keys vc = (Keys)m.WParam.ToInt32();

                Keys keyData = vc | Control.ModifierKeys;

                switch (keyData)
                {
                    case Keys.Tab | Keys.Shift:
                    case Keys.Tab:
                        if (this.RaiseKeyDown(keyData))
                            return true;
                        break;
                }

                switch (keyData)
                {
                    case Keys.Tab | Keys.Shift:
                        return this.PreProcessWmKeyDown_ShiftTab(ref m);
                    case Keys.Tab:
                        return this.PreProcessWmKeyDown_Tab(ref m);
                    default:
                        return base.PreProcessWmKeyDown(ref m);
                }
            }

            protected override Boolean PreProcessWmKeyDown_Left(ref Message m)
            {
                return this.PerformPosMoveLeftByte();
            }

            protected override Boolean PreProcessWmKeyDown_Right(ref Message m)
            {
                return this.PerformPosMoveRightByte();
            }

            #endregion

            #region PreProcessWmChar methods
            public override Boolean PreProcessWmChar(ref Message m)
            {
                if (Control.ModifierKeys == Keys.Control)
                {
                    return this._hexBox.BasePreProcessMessage(ref m);
                }

                Boolean sw = this._hexBox._byteProvider.SupportsWriteByte();
                Boolean si = this._hexBox._byteProvider.SupportsInsertBytes();
                Boolean sd = this._hexBox._byteProvider.SupportsDeleteBytes();

                Int64 pos = this._hexBox._bytePos;
                Int64 sel = this._hexBox._selectionLength;
                Int32 cp = this._hexBox._byteCharacterPos;

                if (
                    (!sw && pos != this._hexBox._byteProvider.Length) ||
                    (!si && pos == this._hexBox._byteProvider.Length))
                {
                    return this._hexBox.BasePreProcessMessage(ref m);
                }

                Char c = (Char)m.WParam.ToInt32();

                if (this.RaiseKeyPress(c))
                    return true;

                if (this._hexBox.ReadOnly)
                    return true;

                Boolean isInsertMode = (pos == this._hexBox._byteProvider.Length);

                // do insert when insertActive = true
                if (!isInsertMode && si && this._hexBox.InsertActive)
                    isInsertMode = true;

                if (sd && si && sel > 0)
                {
                    this._hexBox._byteProvider.DeleteBytes(pos, sel);
                    isInsertMode = true;
                    cp = 0;
                    this._hexBox.SetPosition(pos, cp);
                }

                this._hexBox.ReleaseSelection();

                Byte b = this._hexBox.ByteCharConverter.ToByte(c);
                if (isInsertMode)
                    this._hexBox._byteProvider.InsertBytes(pos, new Byte[] { b });
                else
                    this._hexBox._byteProvider.WriteByte(pos, b);

                this.PerformPosMoveRightByte();
                this._hexBox.Invalidate();

                return true;
            }
            #endregion

            #region Misc
            public override PointF GetCaretPointF(Int64 byteIndex)
            {
                System.Diagnostics.Debug.WriteLine("GetCaretPointF()", "StringKeyInterpreter");

                Point gp = this._hexBox.GetGridBytePoint(byteIndex);
                return this._hexBox.GetByteStringPointF(gp);
            }

            protected override BytePositionInfo GetBytePositionInfo(Point p)
            {
                return this._hexBox.GetStringBytePositionInfo(p);
            }
            #endregion
        }
        #endregion



		#region Paint methods
		/// <summary>
		/// Paints the background.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			switch (this._borderStyle)
			{
				case BorderStyle.Fixed3D:
					{
						if (TextBoxRenderer.IsSupported)
						{
							VisualStyleElement state = VisualStyleElement.TextBox.TextEdit.Normal;
							Color backColor = this.BackColor;

							if (this.Enabled)
							{
								if (this.ReadOnly)
									state = VisualStyleElement.TextBox.TextEdit.ReadOnly;
								else if (this.Focused)
									state = VisualStyleElement.TextBox.TextEdit.Focused;
							}
							else
							{
								state = VisualStyleElement.TextBox.TextEdit.Disabled;
								backColor = this.BackColorDisabled;
							}

							VisualStyleRenderer vsr = new VisualStyleRenderer(state);
							vsr.DrawBackground(e.Graphics, this.ClientRectangle);

							Rectangle rectContent = vsr.GetBackgroundContentRectangle(e.Graphics, this.ClientRectangle);
							e.Graphics.FillRectangle(new SolidBrush(backColor), rectContent);
						}
						else
						{
							// draw background
							e.Graphics.FillRectangle(new SolidBrush(this.BackColor), this.ClientRectangle);

							// draw default border
							ControlPaint.DrawBorder3D(e.Graphics, this.ClientRectangle, Border3DStyle.Sunken);
						}

						break;
					}
				case BorderStyle.FixedSingle:
					{
						// draw background
						e.Graphics.FillRectangle(new SolidBrush(this.BackColor), this.ClientRectangle);

						// draw fixed single border
						ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
						break;
					}
				default:
					{
						// draw background
						e.Graphics.FillRectangle(new SolidBrush(this.BackColor), this.ClientRectangle);
						break;
					}
			}
		}


		/// <summary>
		/// Paints the hex box.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (this._byteProvider == null)
				return;

			System.Diagnostics.Debug.WriteLine("OnPaint " + DateTime.Now.ToString(), "HexBox");

			// draw only in the content rectangle, so exclude the border and the scrollbar.
			Region r = new Region(this.ClientRectangle);
			r.Exclude(this._recContent);
			e.Graphics.ExcludeClip(r);

            this.UpdateVisibilityBytes();


			if (this._lineInfoVisible)
                this.PaintLineInfo(e.Graphics, this._startByte, this._endByte);

			if (!this._stringViewVisible)
			{
                this.PaintHex(e.Graphics, this._startByte, this._endByte);
			}
			else
			{
                this.PaintHexAndStringView(e.Graphics, this._startByte, this._endByte);
				if (this._shadowSelectionVisible)
                    this.PaintCurrentBytesSign(e.Graphics);
			}
			if (this._columnInfoVisible)
                this.PaintHeaderRow(e.Graphics);
			if (this._groupSeparatorVisible)
                this.PaintColumnSeparator(e.Graphics);
		}


		void PaintLineInfo(Graphics g, Int64 startByte, Int64 endByte)
		{
			// Ensure endByte isn't > length of array.
			endByte = Math.Min(this._byteProvider.Length - 1, endByte);

			Color lineInfoColor = (this.InfoForeColor != Color.Empty) ? this.InfoForeColor : this.ForeColor;
			Brush brush = new SolidBrush(lineInfoColor);

            Int32 maxLine = this.GetGridBytePoint(endByte - startByte).Y + 1;

			for (Int32 i = 0; i < maxLine; i++)
			{
                Int64 firstLineByte = (startByte + (this._iHexMaxHBytes) * i) + this._lineInfoOffset;

				PointF bytePointF = this.GetBytePointF(new Point(0, 0 + i));
                String info = firstLineByte.ToString(this._hexStringFormat, System.Threading.Thread.CurrentThread.CurrentCulture);
                Int32 nulls = 8 - info.Length;
                String formattedInfo;
				if (nulls > -1)
				{
					formattedInfo = new String('0', 8 - info.Length) + info;
				}
				else
				{
					formattedInfo = new String('~', 8);
				}

				g.DrawString(formattedInfo, this.Font, brush, new PointF(this._recLineInfo.X, bytePointF.Y), this._stringFormat);
			}
		}

		void PaintHeaderRow(Graphics g)
		{
			Brush brush = new SolidBrush(this.InfoForeColor);
			for (Int32 col = 0; col < this._iHexMaxHBytes; col++)
			{
                this.PaintColumnInfo(g, (Byte)col, brush, col);
			}
		}

		void PaintColumnSeparator(Graphics g)
		{
			for (Int32 col = this.GroupSize; col < this._iHexMaxHBytes; col += this.GroupSize)
			{
				var pen = new Pen(new SolidBrush(this.InfoForeColor), 1);
				PointF headerPointF = this.GetColumnInfoPointF(col);
				headerPointF.X -= this._charSize.Width / 2;
				g.DrawLine(pen, headerPointF, new PointF(headerPointF.X, headerPointF.Y + this._recColumnInfo.Height + this._recHex.Height));
				if (this.StringViewVisible)
				{
					PointF byteStringPointF = this.GetByteStringPointF(new Point(col, 0));
					headerPointF.X -= 2;
					g.DrawLine(pen, new PointF(byteStringPointF.X, byteStringPointF.Y), new PointF(byteStringPointF.X, byteStringPointF.Y + this._recHex.Height));
				}
			}
		}

		void PaintHex(Graphics g, Int64 startByte, Int64 endByte)
		{
			Brush brush = new SolidBrush(this.GetDefaultForeColor());
			Brush selBrush = new SolidBrush(this._selectionForeColor);
			Brush selBrushBack = new SolidBrush(this._selectionBackColor);

            Int32 counter = -1;
            Int64 intern_endByte = Math.Min(this._byteProvider.Length - 1, endByte + this._iHexMaxHBytes);

            Boolean isKeyInterpreterActive = this._keyInterpreter == null || this._keyInterpreter.GetType() == typeof(KeyInterpreter);

			for (Int64 i = startByte; i < intern_endByte + 1; i++)
			{
				counter++;
				Point gridPoint = this.GetGridBytePoint(counter);
                Byte b = this._byteProvider.ReadByte(i);

                Boolean isSelectedByte = i >= this._bytePos && i <= (this._bytePos + this._selectionLength - 1) && this._selectionLength != 0;

				if (isSelectedByte && isKeyInterpreterActive)
				{
                    this.PaintHexStringSelected(g, b, selBrush, selBrushBack, gridPoint);
				}
				else
				{
                    this.PaintHexString(g, b, brush, gridPoint);
				}
			}
		}

		void PaintHexString(Graphics g, Byte b, Brush brush, Point gridPoint)
		{
			PointF bytePointF = this.GetBytePointF(gridPoint);

            String sB = this.ConvertByteToHex(b);

			g.DrawString(sB.Substring(0, 1), this.Font, brush, bytePointF, this._stringFormat);
			bytePointF.X += this._charSize.Width;
			g.DrawString(sB.Substring(1, 1), this.Font, brush, bytePointF, this._stringFormat);
		}

		void PaintColumnInfo(Graphics g, Byte b, Brush brush, Int32 col)
		{
			PointF headerPointF = this.GetColumnInfoPointF(col);

            String sB = this.ConvertByteToHex(b);

			g.DrawString(sB.Substring(0, 1), this.Font, brush, headerPointF, this._stringFormat);
			headerPointF.X += this._charSize.Width;
			g.DrawString(sB.Substring(1, 1), this.Font, brush, headerPointF, this._stringFormat);
		}

		void PaintHexStringSelected(Graphics g, Byte b, Brush brush, Brush brushBack, Point gridPoint)
		{
            String sB = b.ToString(this._hexStringFormat, System.Threading.Thread.CurrentThread.CurrentCulture);
			if (sB.Length == 1)
				sB = "0" + sB;

			PointF bytePointF = this.GetBytePointF(gridPoint);

            Boolean isLastLineChar = (gridPoint.X + 1 == this._iHexMaxHBytes);
            Single bcWidth = (isLastLineChar) ? this._charSize.Width * 2 : this._charSize.Width * 3;

			g.FillRectangle(brushBack, bytePointF.X, bytePointF.Y, bcWidth, this._charSize.Height);
			g.DrawString(sB.Substring(0, 1), this.Font, brush, bytePointF, this._stringFormat);
			bytePointF.X += this._charSize.Width;
			g.DrawString(sB.Substring(1, 1), this.Font, brush, bytePointF, this._stringFormat);
		}

		void PaintHexAndStringView(Graphics g, Int64 startByte, Int64 endByte)
		{
			Brush brush = new SolidBrush(this.GetDefaultForeColor());
			Brush selBrush = new SolidBrush(this._selectionForeColor);
			Brush selBrushBack = new SolidBrush(this._selectionBackColor);

            Int32 counter = -1;
            Int64 intern_endByte = Math.Min(this._byteProvider.Length - 1, endByte + this._iHexMaxHBytes);

            Boolean isKeyInterpreterActive = this._keyInterpreter == null || this._keyInterpreter.GetType() == typeof(KeyInterpreter);
            Boolean isStringKeyInterpreterActive = this._keyInterpreter != null && this._keyInterpreter.GetType() == typeof(StringKeyInterpreter);

			for (Int64 i = startByte; i < intern_endByte + 1; i++)
			{
				counter++;
				Point gridPoint = this.GetGridBytePoint(counter);
				PointF byteStringPointF = this.GetByteStringPointF(gridPoint);
                Byte b = this._byteProvider.ReadByte(i);

                Boolean isSelectedByte = i >= this._bytePos && i <= (this._bytePos + this._selectionLength - 1) && this._selectionLength != 0;

				if (isSelectedByte && isKeyInterpreterActive)
				{
                    this.PaintHexStringSelected(g, b, selBrush, selBrushBack, gridPoint);
				}
				else
				{
                    this.PaintHexString(g, b, brush, gridPoint);
				}

                String s = new String(this.ByteCharConverter.ToChar(b), 1);

				if (isSelectedByte && isStringKeyInterpreterActive)
				{
					g.FillRectangle(selBrushBack, byteStringPointF.X, byteStringPointF.Y, this._charSize.Width, this._charSize.Height);
					g.DrawString(s, this.Font, selBrush, byteStringPointF, this._stringFormat);
				}
				else
				{
					g.DrawString(s, this.Font, brush, byteStringPointF, this._stringFormat);
				}
			}
		}

		void PaintCurrentBytesSign(Graphics g)
		{
			if (this._keyInterpreter != null && this._bytePos != -1 && this.Enabled)
			{
				if (this._keyInterpreter.GetType() == typeof(KeyInterpreter))
				{
					if (this._selectionLength == 0)
					{
						Point gp = this.GetGridBytePoint(this._bytePos - this._startByte);
						PointF pf = this.GetByteStringPointF(gp);
						Size s = new Size((Int32)this._charSize.Width, (Int32)this._charSize.Height);
						Rectangle r = new Rectangle((Int32)pf.X, (Int32)pf.Y, s.Width, s.Height);
						if (r.IntersectsWith(this._recStringView))
						{
							r.Intersect(this._recStringView);
                            this.PaintCurrentByteSign(g, r);
						}
					}
					else
					{
                        Int32 lineWidth = (Int32)(this._recStringView.Width - this._charSize.Width);

						Point startSelGridPoint = this.GetGridBytePoint(this._bytePos - this._startByte);
						PointF startSelPointF = this.GetByteStringPointF(startSelGridPoint);

						Point endSelGridPoint = this.GetGridBytePoint(this._bytePos - this._startByte + this._selectionLength - 1);
						PointF endSelPointF = this.GetByteStringPointF(endSelGridPoint);

                        Int32 multiLine = endSelGridPoint.Y - startSelGridPoint.Y;
						if (multiLine == 0)
						{
							
							Rectangle singleLine = new Rectangle(
								(Int32)startSelPointF.X,
								(Int32)startSelPointF.Y,
								(Int32)(endSelPointF.X - startSelPointF.X + this._charSize.Width),
								(Int32)this._charSize.Height);
							if (singleLine.IntersectsWith(this._recStringView))
							{
								singleLine.Intersect(this._recStringView);
                                this.PaintCurrentByteSign(g, singleLine);
							}
						}
						else
						{
							Rectangle firstLine = new Rectangle(
								(Int32)startSelPointF.X,
								(Int32)startSelPointF.Y,
								(Int32)(this._recStringView.X + lineWidth - startSelPointF.X + this._charSize.Width),
								(Int32)this._charSize.Height);
							if (firstLine.IntersectsWith(this._recStringView))
							{
								firstLine.Intersect(this._recStringView);
                                this.PaintCurrentByteSign(g, firstLine);
							}

							if (multiLine > 1)
							{
								Rectangle betweenLines = new Rectangle(
                                    this._recStringView.X,
									(Int32)(startSelPointF.Y + this._charSize.Height),
									(Int32)(this._recStringView.Width),
									(Int32)(this._charSize.Height * (multiLine - 1)));
								if (betweenLines.IntersectsWith(this._recStringView))
								{
									betweenLines.Intersect(this._recStringView);
                                    this.PaintCurrentByteSign(g, betweenLines);
								}

							}

							Rectangle lastLine = new Rectangle(
                                this._recStringView.X,
								(Int32)endSelPointF.Y,
								(Int32)(endSelPointF.X - this._recStringView.X + this._charSize.Width),
								(Int32)this._charSize.Height);
							if (lastLine.IntersectsWith(this._recStringView))
							{
								lastLine.Intersect(this._recStringView);
                                this.PaintCurrentByteSign(g, lastLine);
							}
						}
					}
				}
				else
				{
					if (this._selectionLength == 0)
					{
						Point gp = this.GetGridBytePoint(this._bytePos - this._startByte);
						PointF pf = this.GetBytePointF(gp);
						Size s = new Size((Int32)this._charSize.Width * 2, (Int32)this._charSize.Height);
						Rectangle r = new Rectangle((Int32)pf.X, (Int32)pf.Y, s.Width, s.Height);
                        this.PaintCurrentByteSign(g, r);
					}
					else
					{
                        Int32 lineWidth = (Int32)(this._recHex.Width - this._charSize.Width * 5);

						Point startSelGridPoint = this.GetGridBytePoint(this._bytePos - this._startByte);
						PointF startSelPointF = this.GetBytePointF(startSelGridPoint);

						Point endSelGridPoint = this.GetGridBytePoint(this._bytePos - this._startByte + this._selectionLength - 1);
						PointF endSelPointF = this.GetBytePointF(endSelGridPoint);

                        Int32 multiLine = endSelGridPoint.Y - startSelGridPoint.Y;
						if (multiLine == 0)
						{
							Rectangle singleLine = new Rectangle(
								(Int32)startSelPointF.X,
								(Int32)startSelPointF.Y,
								(Int32)(endSelPointF.X - startSelPointF.X + this._charSize.Width * 2),
								(Int32)this._charSize.Height);
							if (singleLine.IntersectsWith(this._recHex))
							{
								singleLine.Intersect(this._recHex);
                                this.PaintCurrentByteSign(g, singleLine);
							}
						}
						else
						{
							Rectangle firstLine = new Rectangle(
								(Int32)startSelPointF.X,
								(Int32)startSelPointF.Y,
								(Int32)(this._recHex.X + lineWidth - startSelPointF.X + this._charSize.Width * 2),
								(Int32)this._charSize.Height);
							if (firstLine.IntersectsWith(this._recHex))
							{
								firstLine.Intersect(this._recHex);
                                this.PaintCurrentByteSign(g, firstLine);
							}

							if (multiLine > 1)
							{
								Rectangle betweenLines = new Rectangle(
                                    this._recHex.X,
									(Int32)(startSelPointF.Y + this._charSize.Height),
									(Int32)(lineWidth + this._charSize.Width * 2),
									(Int32)(this._charSize.Height * (multiLine - 1)));
								if (betweenLines.IntersectsWith(this._recHex))
								{
									betweenLines.Intersect(this._recHex);
                                    this.PaintCurrentByteSign(g, betweenLines);
								}

							}

							Rectangle lastLine = new Rectangle(
                                this._recHex.X,
								(Int32)endSelPointF.Y,
								(Int32)(endSelPointF.X - this._recHex.X + this._charSize.Width * 2),
								(Int32)this._charSize.Height);
							if (lastLine.IntersectsWith(this._recHex))
							{
								lastLine.Intersect(this._recHex);
                                this.PaintCurrentByteSign(g, lastLine);
							}
						}
					}
				}
			}
		}

		void PaintCurrentByteSign(Graphics g, Rectangle rec)
		{
			// stack overflowexception on big files - workaround
			if (rec.Top < 0 || rec.Left < 0 || rec.Width <= 0 || rec.Height <= 0)
				return;

			Bitmap myBitmap = new Bitmap(rec.Width, rec.Height);
			Graphics bitmapGraphics = Graphics.FromImage(myBitmap);

			SolidBrush greenBrush = new SolidBrush(this._shadowSelectionColor);

			bitmapGraphics.FillRectangle(greenBrush, 0,
				0, rec.Width, rec.Height);

			g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.GammaCorrected;

			g.DrawImage(myBitmap, rec.Left, rec.Top);
		}

		Color GetDefaultForeColor()
		{
			if (this.Enabled)
				return this.ForeColor;
			else
				return Color.Gray;
		}
		void UpdateVisibilityBytes()
		{
			if (this._byteProvider == null || this._byteProvider.Length == 0)
				return;

            this._startByte = (this._scrollVpos + 1) * this._iHexMaxHBytes - this._iHexMaxHBytes;
            this._endByte = (Int64)Math.Min(this._byteProvider.Length - 1, this._startByte + this._iHexMaxBytes);
		}
		#endregion

		#region Positioning methods
		void UpdateRectanglePositioning()
		{
			// calc char size
            SizeF charSize;
            using (var graphics = this.CreateGraphics())
            {
                charSize = this.CreateGraphics().MeasureString("A", this.Font, 100, this._stringFormat);
            }
            this.CharSize = new SizeF((Single)Math.Ceiling(charSize.Width), (Single)Math.Ceiling(charSize.Height));

            Int32 requiredWidth = 0;

            // calc content bounds
            this._recContent = this.ClientRectangle;
            this._recContent.X += this._recBorderLeft;
            this._recContent.Y += this._recBorderTop;
            this._recContent.Width -= this._recBorderRight + this._recBorderLeft;
            this._recContent.Height -= this._recBorderBottom + this._recBorderTop;

			if (this._vScrollBarVisible)
			{
                this._recContent.Width -= this._vScrollBar.Width;
                this._vScrollBar.Left = this._recContent.X + this._recContent.Width;
                this._vScrollBar.Top = this._recContent.Y;
                this._vScrollBar.Height = this._recContent.Height;
                requiredWidth += this._vScrollBar.Width;
			}

            Int32 marginLeft = 4;

			// calc line info bounds
			if (this._lineInfoVisible)
			{
                this._recLineInfo = new Rectangle(this._recContent.X + marginLeft,
                    this._recContent.Y,
					(Int32)(this._charSize.Width * 10),
                    this._recContent.Height);
                requiredWidth += this._recLineInfo.Width;
			}
			else
			{
                this._recLineInfo = Rectangle.Empty;
                this._recLineInfo.X = marginLeft;
                requiredWidth += marginLeft;
			}

            // calc line info bounds
            this._recColumnInfo = new Rectangle(this._recLineInfo.X + this._recLineInfo.Width, this._recContent.Y, this._recContent.Width - this._recLineInfo.Width, (Int32)charSize.Height + 4);
			if (this._columnInfoVisible)
			{
                this._recLineInfo.Y += (Int32)charSize.Height + 4;
                this._recLineInfo.Height -= (Int32)charSize.Height + 4;
			}
			else
			{
                this._recColumnInfo.Height = 0;
			}

            // calc hex bounds and grid
            this._recHex = new Rectangle(this._recLineInfo.X + this._recLineInfo.Width,
                this._recLineInfo.Y,
                this._recContent.Width - this._recLineInfo.Width,
                this._recContent.Height - this._recColumnInfo.Height);

			if (this.UseFixedBytesPerLine)
			{
                this.SetHorizontalByteCount(this._bytesPerLine);
                this._recHex.Width = (Int32)Math.Floor(((Double)this._iHexMaxHBytes) * this._charSize.Width * 3 + (2 * this._charSize.Width));
                requiredWidth += this._recHex.Width;
			}
			else
			{
                Int32 hmax = (Int32)Math.Floor((Double)this._recHex.Width / (Double)this._charSize.Width);
				if (this._stringViewVisible)
				{
					hmax -= 2;
					if (hmax > 1)
                        this.SetHorizontalByteCount((Int32)Math.Floor((Double)hmax / 4));
					else
                        this.SetHorizontalByteCount(1);
				}
				else
				{
					if (hmax > 1)
                        this.SetHorizontalByteCount((Int32)Math.Floor((Double)hmax / 3));
					else
                        this.SetHorizontalByteCount(1);
				}
                this._recHex.Width = (Int32)Math.Floor(((Double)this._iHexMaxHBytes) * this._charSize.Width * 3 + (2 * this._charSize.Width));
                requiredWidth += this._recHex.Width;
			}

			if (this._stringViewVisible)
			{
                this._recStringView = new Rectangle(this._recHex.X + this._recHex.Width,
                    this._recHex.Y,
					(Int32)(this._charSize.Width * this._iHexMaxHBytes),
                    this._recHex.Height);
                requiredWidth += this._recStringView.Width;
			}
			else
			{
                this._recStringView = Rectangle.Empty;
			}

            this.RequiredWidth = requiredWidth;

            Int32 vmax = (Int32)Math.Floor((Double)this._recHex.Height / (Double)this._charSize.Height);
            this.SetVerticalByteCount(vmax);

            this._iHexMaxBytes = this._iHexMaxHBytes * this._iHexMaxVBytes;

            this.UpdateScrollSize();
		}

		PointF GetBytePointF(Int64 byteIndex)
		{
			Point gp = this.GetGridBytePoint(byteIndex);

			return this.GetBytePointF(gp);
		}

		PointF GetBytePointF(Point gp)
		{
            Single x = (3 * this._charSize.Width) * gp.X + this._recHex.X;
            Single y = (gp.Y + 1) * this._charSize.Height - this._charSize.Height + this._recHex.Y;

			return new PointF(x, y);
		}
		PointF GetColumnInfoPointF(Int32 col)
		{
			Point gp = this.GetGridBytePoint(col);
            Single x = (3 * this._charSize.Width) * gp.X + this._recColumnInfo.X;
            Single y = this._recColumnInfo.Y;

			return new PointF(x, y);
		}

		PointF GetByteStringPointF(Point gp)
		{
            Single x = (this._charSize.Width) * gp.X + this._recStringView.X;
            Single y = (gp.Y + 1) * this._charSize.Height - this._charSize.Height + this._recStringView.Y;

			return new PointF(x, y);
		}

		Point GetGridBytePoint(Int64 byteIndex)
		{
            Int32 row = (Int32)Math.Floor((Double)byteIndex / (Double)this._iHexMaxHBytes);
            Int32 column = (Int32)(byteIndex + this._iHexMaxHBytes - this._iHexMaxHBytes * (row + 1));

			Point res = new Point(column, row);
			return res;
		}
        #endregion

        #region Misc
        /// <summary>
        /// Converts a byte array to a hex string. For example: {10,11} = "0A 0B"
        /// </summary>
        /// <param name="data">the byte array</param>
        /// <returns>the hex string</returns>
        String ConvertBytesToHex(Byte[] data)
		{
			StringBuilder sb = new StringBuilder();
			foreach (Byte b in data)
			{
                String hex = this.ConvertByteToHex(b);
				sb.Append(hex);
				sb.Append(" ");
			}
			if (sb.Length > 0)
				sb.Remove(sb.Length - 1, 1);
            String result = sb.ToString();
			return result;
		}
        /// <summary>
        /// Converts one byte to a hex string. For example: "10" = "0A";
        /// </summary>
        /// <param name="b">the byte to format</param>
        /// <returns>the hex string</returns>
        String ConvertByteToHex(Byte b)
		{
            String sB = b.ToString(this._hexStringFormat, System.Threading.Thread.CurrentThread.CurrentCulture);
			if (sB.Length == 1)
				sB = "0" + sB;
			return sB;
		}
        /// <summary>
        /// Converts a hex string to a byte array. The hex string must be separated by a space char ' '. If there is any invalid hex information in the string the result will be null.
        /// </summary>
        /// <param name="hex">the hex string separated by ' '. For example: "0A 0B 0C"</param>
        /// <returns>the byte array. null if hex is invalid or empty</returns>
        Byte[] ConvertHexToBytes(String hex)
		{
			if (String.IsNullOrEmpty(hex))
				return null;
			hex = hex.Trim();
			var hexArray = hex.Split(' ');
			var byteArray = new Byte[hexArray.Length];

			for (Int32 i = 0; i < hexArray.Length; i++)
			{
				var hexValue = hexArray[i];

                Byte b;
				var isByte = this.ConvertHexToByte(hexValue, out b);
				if (!isByte)
					return null;
				byteArray[i] = b;
			}

			return byteArray;
		}

        Boolean ConvertHexToByte(String hex, out Byte b)
		{
            Boolean isByte = Byte.TryParse(hex, System.Globalization.NumberStyles.HexNumber, System.Threading.Thread.CurrentThread.CurrentCulture, out b);
			return isByte;
		}

		void SetPosition(Int64 bytePos)
		{
            this.SetPosition(bytePos, this._byteCharacterPos);
		}

		void SetPosition(Int64 bytePos, Int32 byteCharacterPos)
		{
			if (this._byteCharacterPos != byteCharacterPos)
			{
                this._byteCharacterPos = byteCharacterPos;
			}

			if (bytePos != this._bytePos)
			{
                this._bytePos = bytePos;
                this.CheckCurrentLineChanged();
                this.CheckCurrentPositionInLineChanged();

                this.OnSelectionStartChanged(EventArgs.Empty);
			}
		}

		void SetSelectionLength(Int64 selectionLength)
		{
			if (selectionLength != this._selectionLength)
			{
                this._selectionLength = selectionLength;
                this.OnSelectionLengthChanged(EventArgs.Empty);
			}
		}

		void SetHorizontalByteCount(Int32 value)
		{
			if (this._iHexMaxHBytes == value)
				return;

            this._iHexMaxHBytes = value;
            this.OnHorizontalByteCountChanged(EventArgs.Empty);
		}

		void SetVerticalByteCount(Int32 value)
		{
			if (this._iHexMaxVBytes == value)
				return;

            this._iHexMaxVBytes = value;
            this.OnVerticalByteCountChanged(EventArgs.Empty);
		}

		void CheckCurrentLineChanged()
		{
            Int64 currentLine = (Int64)Math.Floor((Double)this._bytePos / (Double)this._iHexMaxHBytes) + 1;

			if (this._byteProvider == null && this._currentLine != 0)
			{
                this._currentLine = 0;
                this.OnCurrentLineChanged(EventArgs.Empty);
			}
			else if (currentLine != this._currentLine)
			{
                this._currentLine = currentLine;
                this.OnCurrentLineChanged(EventArgs.Empty);
			}
		}

		void CheckCurrentPositionInLineChanged()
		{
			Point gb = this.GetGridBytePoint(this._bytePos);
            Int32 currentPositionInLine = gb.X + 1;

			if (this._byteProvider == null && this._currentPositionInLine != 0)
			{
                this._currentPositionInLine = 0;
                this.OnCurrentPositionInLineChanged(EventArgs.Empty);
			}
			else if (currentPositionInLine != this._currentPositionInLine)
			{
                this._currentPositionInLine = currentPositionInLine;
                this.OnCurrentPositionInLineChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// Raises the InsertActiveChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnInsertActiveChanged(EventArgs e)
		{
			if (InsertActiveChanged != null)
				InsertActiveChanged(this, e);
		}

		/// <summary>
		/// Raises the ReadOnlyChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnReadOnlyChanged(EventArgs e)
		{
			if (ReadOnlyChanged != null)
				ReadOnlyChanged(this, e);
		}

		/// <summary>
		/// Raises the ByteProviderChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnByteProviderChanged(EventArgs e)
		{
			if (ByteProviderChanged != null)
				ByteProviderChanged(this, e);
		}

		/// <summary>
		/// Raises the SelectionStartChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnSelectionStartChanged(EventArgs e)
		{
			if (SelectionStartChanged != null)
				SelectionStartChanged(this, e);
		}

		/// <summary>
		/// Raises the SelectionLengthChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnSelectionLengthChanged(EventArgs e)
		{
			if (SelectionLengthChanged != null)
				SelectionLengthChanged(this, e);
		}

		/// <summary>
		/// Raises the LineInfoVisibleChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnLineInfoVisibleChanged(EventArgs e)
		{
			if (LineInfoVisibleChanged != null)
				LineInfoVisibleChanged(this, e);
		}

		/// <summary>
		/// Raises the OnColumnInfoVisibleChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnColumnInfoVisibleChanged(EventArgs e)
		{
			if (ColumnInfoVisibleChanged != null)
				ColumnInfoVisibleChanged(this, e);
		}

		/// <summary>
		/// Raises the ColumnSeparatorVisibleChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnGroupSeparatorVisibleChanged(EventArgs e)
		{
			if (GroupSeparatorVisibleChanged != null)
				GroupSeparatorVisibleChanged(this, e);
		}

		/// <summary>
		/// Raises the StringViewVisibleChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnStringViewVisibleChanged(EventArgs e)
		{
			if (StringViewVisibleChanged != null)
				StringViewVisibleChanged(this, e);
		}

		/// <summary>
		/// Raises the BorderStyleChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnBorderStyleChanged(EventArgs e)
		{
			if (BorderStyleChanged != null)
				BorderStyleChanged(this, e);
		}

		/// <summary>
		/// Raises the UseFixedBytesPerLineChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnUseFixedBytesPerLineChanged(EventArgs e)
		{
			if (UseFixedBytesPerLineChanged != null)
				UseFixedBytesPerLineChanged(this, e);
		}

		/// <summary>
		/// Raises the GroupSizeChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnGroupSizeChanged(EventArgs e)
		{
			if (GroupSizeChanged != null)
				GroupSizeChanged(this, e);
		}

		/// <summary>
		/// Raises the BytesPerLineChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnBytesPerLineChanged(EventArgs e)
		{
			if (BytesPerLineChanged != null)
				BytesPerLineChanged(this, e);
		}

		/// <summary>
		/// Raises the VScrollBarVisibleChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnVScrollBarVisibleChanged(EventArgs e)
		{
			if (VScrollBarVisibleChanged != null)
				VScrollBarVisibleChanged(this, e);
		}

		/// <summary>
		/// Raises the HexCasingChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnHexCasingChanged(EventArgs e)
		{
			if (HexCasingChanged != null)
				HexCasingChanged(this, e);
		}

		/// <summary>
		/// Raises the HorizontalByteCountChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnHorizontalByteCountChanged(EventArgs e)
		{
			if (HorizontalByteCountChanged != null)
				HorizontalByteCountChanged(this, e);
		}

		/// <summary>
		/// Raises the VerticalByteCountChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnVerticalByteCountChanged(EventArgs e)
		{
			if (VerticalByteCountChanged != null)
				VerticalByteCountChanged(this, e);
		}

		/// <summary>
		/// Raises the CurrentLineChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnCurrentLineChanged(EventArgs e)
		{
			if (CurrentLineChanged != null)
				CurrentLineChanged(this, e);
		}

		/// <summary>
		/// Raises the CurrentPositionInLineChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnCurrentPositionInLineChanged(EventArgs e)
		{
			if (CurrentPositionInLineChanged != null)
				CurrentPositionInLineChanged(this, e);
		}


		/// <summary>
		/// Raises the Copied event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnCopied(EventArgs e)
		{
			if (Copied != null)
				Copied(this, e);
		}

		/// <summary>
		/// Raises the CopiedHex event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnCopiedHex(EventArgs e)
		{
			if (CopiedHex != null)
				CopiedHex(this, e);
		}

		/// <summary>
		/// Raises the MouseDown event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("OnMouseDown()", "HexBox");

			if (!this.Focused)
                this.Focus();

			if (e.Button == MouseButtons.Left)
                this.SetCaretPosition(new Point(e.X, e.Y));

			base.OnMouseDown(e);
		}

		/// <summary>
		/// Raises the MouseWhell event
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
            Int32 linesToScroll = -(e.Delta * SystemInformation.MouseWheelScrollLines / 120);
			this.PerformScrollLines(linesToScroll);

			base.OnMouseWheel(e);
		}


		/// <summary>
		/// Raises the Resize event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
            this.UpdateRectanglePositioning();
		}

		/// <summary>
		/// Raises the GotFocus event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnGotFocus(EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("OnGotFocus()", "HexBox");

			base.OnGotFocus(e);

            this.CreateCaret();
		}

		/// <summary>
		/// Raises the LostFocus event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnLostFocus(EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("OnLostFocus()", "HexBox");

			base.OnLostFocus(e);

            this.DestroyCaret();
		}

		void _byteProvider_LengthChanged(Object sender, EventArgs e)
		{
            this.UpdateScrollSize();
		}
		#endregion
    }
}
