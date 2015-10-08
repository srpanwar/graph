﻿using System;

namespace com.yahoo.search.xsd
{

    //------------------------------------------------------------------------------
    // <auto-generated>
    //     This code was generated by a tool.
    //     Runtime Version:2.0.50727.4908
    //
    //     Changes to this file may cause incorrect behavior and will be lost if
    //     the code is regenerated.
    // </auto-generated>
    //------------------------------------------------------------------------------

    using System.Xml.Serialization;

    // 
    // This source code was auto-generated by xsd, Version=2.0.50727.1432.
    // 


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.inktomi.com/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.inktomi.com/", IsNullable = false)]
    public partial class ysearchresponse
    {

        private string nextpageField;

        private ysearchresponseResultset_web resultset_webField;

        private byte responsecodeField;

        /// <remarks/>
        public string nextpage
        {
            get
            {
                return this.nextpageField;
            }
            set
            {
                this.nextpageField = value;
            }
        }

        /// <remarks/>
        public ysearchresponseResultset_web resultset_web
        {
            get
            {
                return this.resultset_webField;
            }
            set
            {
                this.resultset_webField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte responsecode
        {
            get
            {
                return this.responsecodeField;
            }
            set
            {
                this.responsecodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.inktomi.com/")]
    public partial class ysearchresponseResultset_web
    {

        private ysearchresponseResultset_webResult[] resultField;

        private byte countField;

        private byte startField;

        private Int64 totalhitsField;

        private Int64 deephitsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("result")]
        public ysearchresponseResultset_webResult[] result
        {
            get
            {
                return this.resultField;
            }
            set
            {
                this.resultField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte start
        {
            get
            {
                return this.startField;
            }
            set
            {
                this.startField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public Int64 totalhits
        {
            get
            {
                return this.totalhitsField;
            }
            set
            {
                unchecked
                {
                    this.totalhitsField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public Int64 deephits
        {
            get
            {
                return this.deephitsField;
            }
            set
            {
                unchecked
                {
                    this.deephitsField = value;
                }
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.inktomi.com/")]
    public partial class ysearchresponseResultset_webResult
    {

        private string abstractField;

        private string clickurlField;

        private string dateField;

        private string dispurlField;

        private Int64 sizeField;

        private string titleField;

        private string urlField;

        /// <remarks/>
        public string @abstract
        {
            get
            {
                return this.abstractField;
            }
            set
            {
                this.abstractField = value;
            }
        }

        /// <remarks/>
        public string clickurl
        {
            get
            {
                return this.clickurlField;
            }
            set
            {
                this.clickurlField = value;
            }
        }

        /// <remarks/>
        public string date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        /// <remarks/>
        public string dispurl
        {
            get
            {
                return this.dispurlField;
            }
            set
            {
                this.dispurlField = value;
            }
        }

        /// <remarks/>
        public Int64 size
        {
            get
            {
                return this.sizeField;
            }
            set
            {
                unchecked
                {
                    this.sizeField = value;
                }
            }
        }

        /// <remarks/>
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        public string url
        {
            get
            {
                return this.urlField;
            }
            set
            {
                this.urlField = value;
            }
        }
    }
}