﻿namespace org.musicbrainz.www.xsd.artistns
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://musicbrainz.org/ns/mmd-1.0#")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://musicbrainz.org/ns/mmd-1.0#", IsNullable = false)]
    public partial class metadata
    {

        private metadataArtistlist artistlistField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("artist-list")]
        public metadataArtistlist artistlist
        {
            get
            {
                return this.artistlistField;
            }
            set
            {
                this.artistlistField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://musicbrainz.org/ns/mmd-1.0#")]
    public partial class metadataArtistlist
    {

        private metadataArtistlistArtist[] artistField;

        private ushort countField;

        private byte offsetField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("artist")]
        public metadataArtistlistArtist[] artist
        {
            get
            {
                return this.artistField;
            }
            set
            {
                this.artistField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort count
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
        public byte offset
        {
            get
            {
                return this.offsetField;
            }
            set
            {
                this.offsetField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://musicbrainz.org/ns/mmd-1.0#")]
    public partial class metadataArtistlistArtist
    {

        private string nameField;

        private string sortnameField;

        private metadataArtistlistArtistLifespan lifespanField;

        private string idField;

        private string typeField;

        private byte scoreField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("sort-name")]
        public string sortname
        {
            get
            {
                return this.sortnameField;
            }
            set
            {
                this.sortnameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("life-span")]
        public metadataArtistlistArtistLifespan lifespan
        {
            get
            {
                return this.lifespanField;
            }
            set
            {
                this.lifespanField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://musicbrainz.org/ns/ext-1.0#")]
        public byte score
        {
            get
            {
                return this.scoreField;
            }
            set
            {
                this.scoreField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://musicbrainz.org/ns/mmd-1.0#")]
    public partial class metadataArtistlistArtistLifespan
    {

        private System.DateTime beginField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime begin
        {
            get
            {
                return this.beginField;
            }
            set
            {
                this.beginField = value;
            }
        }
    }
}