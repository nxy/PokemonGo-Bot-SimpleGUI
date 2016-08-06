
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class Pokedex
{

    private PokedexPokemon[] pokemonField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Pokemon")]
    public PokedexPokemon[] Pokemon
    {
        get
        {
            return this.pokemonField;
        }
        set
        {
            this.pokemonField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class PokedexPokemon
{

    private string type1Field;

    private string type2Field;

    private ushort indexField;

    private string nameField;

    /// <remarks/>
    public string Type1
    {
        get
        {
            return this.type1Field;
        }
        set
        {
            this.type1Field = value;
        }
    }

    /// <remarks/>
    public string Type2
    {
        get
        {
            return this.type2Field;
        }
        set
        {
            this.type2Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public ushort Index
    {
        get
        {
            return this.indexField;
        }
        set
        {
            this.indexField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
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
}

