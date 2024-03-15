using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class Collection<T>
{
    /// <summary>
    /// Nom de la collection.
    /// </summary>
    [Header("Collection Settings")]
    [SerializeField]
    private string collectionName;

    /// <summary>
    /// Liste des �l�ments de la collection.
    /// </summary>
    [Header("Collection Elements")]
    [SerializeField]
    private List<T> list = new List<T>();

    /// <summary>
    /// Dictionnaire associant des cl�s uniques � des �l�ments de la collection.
    /// </summary>
    [SerializeField]
    private SerializableDictionary<string, T> dictionary = new SerializableDictionary<string, T>();

    #region Events

    /// <summary>
    /// D�l�gu� d'�v�nement appel� lorsqu'un nouvel �l�ment est ajout� � la collection.
    /// </summary>
    /// <param name="item">L'�l�ment ajout� � la collection.</param>
    public delegate void ItemAddedEventHandler(T item);

    /// <summary>
    /// D�l�gu� d'�v�nement appel� lorsqu'un �l�ment est retir� de la collection.
    /// </summary>
    /// <param name="item">L'�l�ment retir� de la collection.</param>
    public delegate void ItemRemovedEventHandler(T item);

    /// <summary>
    /// D�l�gu� d'�v�nement appel� lorsqu'une collection est enti�rement vid�e.
    /// </summary>
    public delegate void CollectionClearedEventHandler();

    /// <summary>
    /// �v�nement d�clench� lorsqu'un nouvel �l�ment est ajout� � la collection.
    /// </summary>
    public event ItemAddedEventHandler ItemAdded;

    /// <summary>
    /// �v�nement d�clench� lorsqu'un �l�ment est retir� de la collection.
    /// </summary>
    public event ItemRemovedEventHandler ItemRemoved;

    /// <summary>
    /// �v�nement d�clench� lorsqu'une collection est enti�rement vid�e.
    /// </summary>
    public event CollectionClearedEventHandler CollectionCleared;

    #endregion

    #region Index

    /// <summary>
    /// V�rifie si l'index sp�cifi� est en dehors de la plage valide des indices de la collection.
    /// </summary>
    /// <param name="index">L'index � v�rifier.</param>
    /// <returns>true si l'index est en dehors de la plage valide, sinon false.</returns>
    public bool IsOutOfIndex(int index)
    {
        return index < 0 || index >= list.Count;
    }

    /// <summary>
    /// V�rifie si l'index sp�cifi� est � l'int�rieur de la plage valide des indices de la collection.
    /// </summary>
    /// <param name="index">L'index � v�rifier.</param>
    /// <returns>true si l'index est � l'int�rieur de la plage valide, sinon false.</returns>
    public bool IsInOfIndex(int index)
    {
        return index >= 0 && index < list.Count;
    }

    #endregion

    #region Has

    /// <summary>
    /// V�rifie si la collection contient un �l�ment avec la cl� sp�cifi�e.
    /// </summary>
    /// <param name="key">La cl� de l'�l�ment � rechercher dans la collection.</param>
    /// <returns>true si la collection contient un �l�ment avec la cl� sp�cifi�e, sinon false.</returns>
    public bool HasItem(string key)
    {
        if (dictionary.ContainsKey(key)) return true;
        return false;
    }

    /// <summary>
    /// V�rifie si la collection contient l'�l�ment sp�cifi�.
    /// </summary>
    /// <param name="item">L'�l�ment � rechercher dans la collection.</param>
    /// <returns>true si la collection contient l'�l�ment sp�cifi�, sinon false.</returns>
    public bool HasItem(T item)
    {
        if (list.Contains(item)) return true;
        return false;
    }

    #endregion

    #region Add

    /// <summary>
    /// Ajoute un �l�ment � la collection.
    /// </summary>
    /// <param name="item">L'�l�ment entr�.</param>
    /// /// <exception cref="ArgumentException">Lev�e si une cl� avec le m�me nom existe d�j� dans la collection.</exception>
    public void AddToList(T item)
    {
        string key = item.ToString();
        if (!HasItem(key))
        {
            list.Add(item);
            dictionary.Add(key, item);
            ItemAdded?.Invoke(item);
        }
        else
        {
            throw new ArgumentException("Une cl� avec ce nom existe d�j� dans la collection.");
        }
    }

    public void AddItemToDictionary(T item)
    {
        string key = item.ToString();
        if (!HasItem(key))
        {
            dictionary.Add(key, item);
        }
    }

    /// <summary>
    /// Ajoute un �l�ment d�faut � la collection.
    /// </summary>
    /// <param name="key">La cl� associ�e � l'�l�ment par d�faut.</param>
    /// /// <exception cref="ArgumentException">Lev�e si une cl� avec le m�me nom existe d�j� dans la collection.</exception>
    public void AddItem(string key)
    {
        if (!HasItem(key))
        {
            T item = default;
            list.Add(item);
            dictionary.Add(key, item);
            ItemAdded?.Invoke(item);
        }
        else
        {
            throw new ArgumentException("Une cl� avec ce nom existe d�j� dans la collection.");
        }
    }

    /// <summary>
    /// Ajoute un nouvel �l�ment � la collection avec la cl� sp�cifi�e.
    /// </summary>
    /// <param name="key">La cl� de l'�l�ment � ajouter.</param>
    /// <param name="item">L'�l�ment � ajouter � la collection.</param>
    /// <exception cref="ArgumentException">Lev�e si une cl� avec le m�me nom existe d�j� dans la collection.</exception>
    public void AddItem(string key, T item)
    {
        if (!HasItem(key))
        {
            list.Add(item);
            dictionary.Add(key, item);
            ItemAdded?.Invoke(item); // D�clenche l'�v�nement ItemAdded pour notifier les abonn�s.
        }
        else
        {
            throw new ArgumentException("Une cl� avec ce nom existe d�j� dans la collection.");
        }
    }

    /// <summary>
    /// Ajoute une s�rie d'�l�ments � la collection avec des cl�s g�n�r�es � partir de la cl� sp�cifi�e.
    /// </summary>
    /// <param name="key">La cl� de base pour g�n�rer les cl�s des �l�ments ajout�s.</param>
    /// <param name="items">Les �l�ments � ajouter � la collection.</param>
    /// <exception cref="ArgumentException">Lev�e si une cl� avec le m�me nom existe d�j� dans la collection, ce qui emp�che de cr�er une succession de cl�s � partir de ce nom.</exception>
    public void AddItems(string key, IEnumerable<T> items)
    {
        if (!HasItem(key))
        {
            int index = 0;
            foreach (T item in items)
            {
                string uniquekey = key + index;
                if (!HasItem(uniquekey))
                {
                    list.Add(item);
                    dictionary.Add(uniquekey, item);
                    ItemAdded?.Invoke(item);
                }
                index++;
            }
        }
        else
        {
            throw new ArgumentException("Une cl� avec ce nom existe d�j� dans la collection, impossible de cr�er une succession de cl� � partir de ce nom.");
        }
    }

    #endregion

    #region Getter

    /// <summary>
    /// R�cup�re le nom de la collection.
    /// </summary>
    /// <returns>Le nom de la collection.</returns>
    public string GetCollectionName() { return collectionName; }

    /// <summary>
    /// R�cup�re la cl� associ�e � l'�l�ment sp�cifi� dans la collection.
    /// </summary>
    /// <param name="item">L'�l�ment dont la cl� doit �tre r�cup�r�e.</param>
    /// <returns>La cl� associ�e � l'�l�ment, ou null si aucun �l�ment correspondant n'est trouv�.</returns>
    public string GetKeyByItem(T item)
    {
        foreach (var kvp in dictionary)
            if (EqualityComparer<T>.Default.Equals(kvp.Value, item)) return kvp.Key;
        return null;
    }

    /// <summary>
    /// R�cup�re l'�l�ment associ� � la cl� sp�cifi�e dans la collection.
    /// </summary>
    /// <param name="key">La cl� de l'�l�ment � r�cup�rer.</param>
    /// <returns>
    /// L'�l�ment associ� � la cl� sp�cifi�e s'il est trouv� dans la collection,
    /// sinon la valeur par d�faut de type T.
    /// </returns>
    public T GetItemBykey(string key)
    {
        if (HasItem(key)) return dictionary[key];
        return default(T);
    }

    /// <summary>
    /// R�cup�re l'�l�ment � l'index sp�cifi� dans la collection.
    /// </summary>
    /// <param name="index">L'index de l'�l�ment � r�cup�rer.</param>
    /// <returns>
    /// L'�l�ment � l'index sp�cifi� dans la collection, ou la valeur par d�faut de type T si l'index est invalide.
    /// </returns>
    public T GetItemByIndex(int index)
    {
        if (IsOutOfIndex(index)) return default(T);
        return list[index];
    }

    /// <summary>
    /// R�cup�re une liste contenant tous les �l�ments de la collection.
    /// </summary>
    /// <returns>Une liste contenant tous les �l�ments de la collection.</returns>
    public List<T> GetItemsList() { return list; }

    /// <summary>
    /// R�cup�re un dictionnaire contenant toutes les cl�s et leurs �l�ments correspondants de la collection.
    /// </summary>
    /// <returns>Un dictionnaire contenant toutes les cl�s et leurs �l�ments correspondants de la collection.</returns>
    public SerializableDictionary<string, T> GetItemsDictionary() { return dictionary; }

    /// <summary>
    /// R�cup�re le nombre d'�l�ments dans la collection.
    /// </summary>
    /// <returns>Le nombre d'�l�ments dans la collection.</returns>
    public int GetItemsNumber() { return list.Count; }

    #endregion

    #region Find

    /// <summary>
    /// Recherche et r�cup�re l'�l�ment associ� � la cl� sp�cifi�e dans la collection.
    /// </summary>
    /// <param name="key">La cl� de l'�l�ment � rechercher.</param>
    /// <returns>
    /// L'�l�ment associ� � la cl� sp�cifi�e s'il est trouv� dans la collection,
    /// sinon la valeur par d�faut de type T.
    /// </returns>
    public T FindItemBykey(string key)
    {
        if (HasItem(key)) return dictionary[key];
        return default(T);
    }

    /// <summary>
    /// Recherche et r�cup�re l'�l�ment � l'index sp�cifi� dans la collection.
    /// </summary>
    /// <param name="index">L'index de l'�l�ment � rechercher.</param>
    /// <returns>
    /// L'�l�ment � l'index sp�cifi� dans la collection,
    /// ou la valeur par d�faut de type T si l'index est invalide.
    /// </returns>
    public T FindItemByIndex(int index)
    {
        if (IsInOfIndex(index)) return list[index];
        return default(T);
    }

    /// <summary>
    /// Recherche et r�cup�re la cl� associ�e � l'�l�ment sp�cifi� dans la collection.
    /// </summary>
    /// <param name="item">L'�l�ment dont la cl� doit �tre recherch�e.</param>
    /// <returns>
    /// La cl� associ�e � l'�l�ment sp�cifi�, ou null si aucun �l�ment correspondant n'est trouv�.
    /// </returns>
    public string FindKeyByItem(T item)
    {
        string keyToFind = null;
        foreach (var kvp in dictionary)
        {
            if (EqualityComparer<T>.Default.Equals(kvp.Value, item))
            {
                keyToFind = kvp.Key;
                break;
            }
        }
        return keyToFind;
    }

    /// <summary>
    /// Recherche et r�cup�re tous les �l�ments de la collection qui correspondent au pr�dicat sp�cifi�.
    /// </summary>
    /// <param name="predicate">Le pr�dicat utilis� pour d�terminer les �l�ments � r�cup�rer.</param>
    /// <returns>Une liste contenant tous les �l�ments de la collection qui correspondent au pr�dicat sp�cifi�.</returns>
    public List<T> FindItems(Predicate<T> predicate)
    {
        return list.FindAll(predicate);
    }

    /// <summary>
    /// Recherche et r�cup�re tous les �l�ments de la collection qui satisfont la condition sp�cifi�e.
    /// </summary>
    /// <param name="condition">La fonction qui sp�cifie la condition � v�rifier pour chaque �l�ment.</param>
    /// <returns>Une liste contenant tous les �l�ments de la collection qui satisfont la condition sp�cifi�e.</returns>
    public List<T> FindItems(Func<T, bool> condition)
    {
        return list.Where(condition).ToList();
    }

    /// <summary>
    /// Recherche et r�cup�re tous les �l�ments de la collection qui ont une propri�t� sp�cifique �gale � une valeur donn�e.
    /// </summary>
    /// <param name="propertySelector">La fonction de s�lection de propri�t� utilis�e pour extraire la propri�t� sp�cifique de chaque �l�ment.</param>
    /// <param name="value">La valeur � comparer avec la propri�t� sp�cifique des �l�ments.</param>
    /// <returns>Une liste contenant tous les �l�ments de la collection dont la propri�t� sp�cifique est �gale � la valeur donn�e.</returns>
    public List<T> FindItemsByProperty(Func<T, object> propertySelector, object value)
    {
        return list.Where(item => propertySelector(item).Equals(value)).ToList();
    }

    /// <summary>
    /// Recherche et r�cup�re tous les �l�ments de la collection dont la propri�t� sp�cifi�e contient une correspondance partielle avec un motif donn�.
    /// </summary>
    /// <param name="propertySelector">La fonction de s�lection de propri�t� utilis�e pour extraire la propri�t� sp�cifique de chaque �l�ment.</param>
    /// <param name="pattern">Le motif � rechercher dans la propri�t� sp�cifique des �l�ments.</param>
    /// <returns>Une liste contenant tous les �l�ments de la collection dont la propri�t� sp�cifi�e contient une correspondance partielle avec le motif donn�.</returns>
    public List<T> FindItemsByPartialMatch(Func<T, string> propertySelector, string pattern)
    {
        return list.Where(item => propertySelector(item).Contains(pattern)).ToList();
    }

    /// <summary>
    /// Recherche et r�cup�re tous les �l�ments de la collection dont la propri�t� sp�cifi�e a une valeur minimale �gale ou sup�rieure � une valeur donn�e.
    /// </summary>
    /// <param name="propertySelector">La fonction de s�lection de propri�t� utilis�e pour extraire la propri�t� sp�cifique de chaque �l�ment.</param>
    /// <param name="minValue">La valeur minimale � comparer avec la propri�t� sp�cifique des �l�ments.</param>
    /// <returns>Une liste contenant tous les �l�ments de la collection dont la propri�t� sp�cifi�e a une valeur minimale �gale ou sup�rieure � la valeur donn�e.</returns>
    public List<T> FindItemsByMinValue(Func<T, int> propertySelector, int minValue)
    {
        return list.Where(item => propertySelector(item) >= minValue).ToList();
    }

    /// <summary>
    /// Recherche et r�cup�re tous les �l�ments de la collection dont la propri�t� sp�cifi�e a une valeur maximale �gale ou inf�rieure � une valeur donn�e.
    /// </summary>
    /// <param name="propertySelector">La fonction de s�lection de propri�t� utilis�e pour extraire la propri�t� sp�cifique de chaque �l�ment.</param>
    /// <param name="maxValue">La valeur maximale � comparer avec la propri�t� sp�cifique des �l�ments.</param>
    /// <returns>Une liste contenant tous les �l�ments de la collection dont la propri�t� sp�cifi�e a une valeur maximale �gale ou inf�rieure � la valeur donn�e.</returns>
    public List<T> FindItemsByMaxValue(Func<T, int> propertySelector, int maxValue)
    {
        return list.Where(item => propertySelector(item) <= maxValue).ToList();
    }

    #endregion

    #region Setter

    /// <summary>
    /// D�finit le nom de la collection.
    /// </summary>
    /// <param name="value">Le nouveau nom de la collection.</param>
    public void SetCollectionName(string value) { collectionName = value; }

    #endregion

    #region Remove

    /// <summary>
    /// Supprime l'�l�ment associ� � la cl� sp�cifi�e de la collection.
    /// </summary>
    /// <param name="key">La cl� de l'�l�ment � supprimer.</param>
    /// <exception cref="ArgumentException">Lev�e si aucune cl� correspondante n'est trouv�e dans la collection.</exception>
    public void RemoveItemBykey(string key)
    {
        if (HasItem(key))
        {
            T item = FindItemBykey(key);
            list.Remove(item);
            dictionary.Remove(key);
            ItemRemoved?.Invoke(item);
        }
        else
        {
            throw new ArgumentException("Une cl� avec ce nom n'existe pas dans la collection.");
        }
    }

    /// <summary>
    /// Supprime l'�l�ment sp�cifi� de la collection.
    /// </summary>
    /// <param name="item">L'�l�ment � supprimer de la collection.</param>
    /// <exception cref="ArgumentException">Lev�e si l'�l�ment sp�cifi� n'est pas pr�sent dans la collection.</exception>
    public void RemoveItemBySelf(T item)
    {
        if (HasItem(item))
        {
            list.Remove(item);
            dictionary.Remove(FindKeyByItem(item));
            ItemRemoved?.Invoke(item);
        }
        else
        {
            throw new ArgumentException("Cet item n'existe pas dans la collection.");
        }
    }

    /// <summary>
    /// Supprime tous les �l�ments sp�cifi�s de la collection.
    /// </summary>
    /// <param name="items">Une collection d'�l�ments � supprimer de la collection.</param>
    /// <remarks>
    /// Cette m�thode parcourt la liste des �l�ments sp�cifi�s et supprime chaque �l�ment de la collection.
    /// </remarks>
    public void RemoveItemsBySelf(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            RemoveItemBySelf(item);
        }
    }

    /// <summary>
    /// Supprime tous les �l�ments associ�s aux cl�s sp�cifi�es de la collection.
    /// </summary>
    /// <param name="keys">Une collection de cl�s � supprimer de la collection.</param>
    /// <remarks>
    /// Cette m�thode parcourt la liste des cl�s sp�cifi�es et supprime chaque �l�ment associ� � chaque cl� de la collection.
    /// </remarks>
    public void RemoveItemsByKey(IEnumerable<string> keys)
    {
        foreach (var key in keys)
        {
            RemoveItemBykey(key);
        }
    }

    #endregion

    #region Clear

    /// <summary>
    /// Supprime tous les �l�ments de la collection.
    /// </summary>
    /// <remarks>
    /// Cette m�thode vide � la fois la liste et le dictionnaire de la collection.
    /// Elle d�clenche �galement l'�v�nement <see cref="CollectionCleared"/> apr�s avoir vid� la collection.
    /// </remarks>
    public void ClearItems()
    {
        list.Clear();
        dictionary.Clear();
        CollectionCleared?.Invoke();
    }

    #endregion

    #region Save

    /// <summary>
    /// S�rialise la liste d'�l�ments de la collection et les sauvegarde dans un fichier binaire sp�cifi�.
    /// </summary>
    /// <param name="filePath">Le chemin d'acc�s complet du fichier de sauvegarde.</param>
    public void Save(string filePath)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            formatter.Serialize(stream, list);
        }
    }

    #endregion

    #region Load

    /// <summary>
    /// Charge une liste d'�l�ments � partir d'un fichier binaire sp�cifi� et les assigne � la collection.
    /// </summary>
    /// <param name="filePath">Le chemin d'acc�s complet du fichier de sauvegarde.</param>
    public void Load(string filePath)
    {
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                list = (List<T>)formatter.Deserialize(stream);
            }
        }
        else
        {
            Debug.LogError("Le fichier de sauvegarde n'existe pas.");
        }
    }

    #endregion

    public void OnDestroy()
    {
        ClearItems();
    }
}


[System.Serializable]
public class Collection<U, T>
{
    /// <summary>
    /// Nom de la collection.
    /// </summary>
    [Header("Collection Settings")]
    [SerializeField]
    private string collectionName;

    /// <summary>
    /// Liste des �l�ments de la collection.
    /// </summary>
    [Header("Collection Elements")]
    [SerializeField]
    private List<T> list = new List<T>();

    /// <summary>
    /// Dictionnaire associant des cl�s uniques � des �l�ments de la collection.
    /// </summary>
    [SerializeField]
    private SerializableDictionary<U, T> dictionary = new SerializableDictionary<U, T>();

    #region Events

    /// <summary>
    /// D�l�gu� d'�v�nement appel� lorsqu'un nouvel �l�ment est ajout� � la collection.
    /// </summary>
    /// <param name="item">L'�l�ment ajout� � la collection.</param>
    public delegate void ItemAddedEventHandler(T item);

    /// <summary>
    /// D�l�gu� d'�v�nement appel� lorsqu'un �l�ment est retir� de la collection.
    /// </summary>
    /// <param name="item">L'�l�ment retir� de la collection.</param>
    public delegate void ItemRemovedEventHandler(T item);

    /// <summary>
    /// D�l�gu� d'�v�nement appel� lorsqu'une collection est enti�rement vid�e.
    /// </summary>
    public delegate void CollectionClearedEventHandler();

    /// <summary>
    /// �v�nement d�clench� lorsqu'un nouvel �l�ment est ajout� � la collection.
    /// </summary>
    public event ItemAddedEventHandler ItemAdded;

    /// <summary>
    /// �v�nement d�clench� lorsqu'un �l�ment est retir� de la collection.
    /// </summary>
    public event ItemRemovedEventHandler ItemRemoved;

    /// <summary>
    /// �v�nement d�clench� lorsqu'une collection est enti�rement vid�e.
    /// </summary>
    public event CollectionClearedEventHandler CollectionCleared;

    #endregion

    #region Index

    /// <summary>
    /// V�rifie si l'index sp�cifi� est en dehors de la plage valide des indices de la collection.
    /// </summary>
    /// <param name="index">L'index � v�rifier.</param>
    /// <returns>true si l'index est en dehors de la plage valide, sinon false.</returns>
    public bool IsOutOfIndex(int index)
    {
        return index < 0 || index >= list.Count;
    }

    /// <summary>
    /// V�rifie si l'index sp�cifi� est � l'int�rieur de la plage valide des indices de la collection.
    /// </summary>
    /// <param name="index">L'index � v�rifier.</param>
    /// <returns>true si l'index est � l'int�rieur de la plage valide, sinon false.</returns>
    public bool IsInOfIndex(int index)
    {
        return index >= 0 && index < list.Count;
    }

    #endregion

    #region Has

    /// <summary>
    /// V�rifie si la collection contient un �l�ment avec la cl� sp�cifi�e.
    /// </summary>
    /// <param name="key">La cl� de l'�l�ment � rechercher dans la collection.</param>
    /// <returns>true si la collection contient un �l�ment avec la cl� sp�cifi�e, sinon false.</returns>
    public bool HasItem(U key)
    {
        if (dictionary.ContainsKey(key)) return true;
        return false;
    }

    /// <summary>
    /// V�rifie si la collection contient l'�l�ment sp�cifi�.
    /// </summary>
    /// <param name="item">L'�l�ment � rechercher dans la collection.</param>
    /// <returns>true si la collection contient l'�l�ment sp�cifi�, sinon false.</returns>
    public bool HasItem(T item)
    {
        if (list.Contains(item)) return true;
        return false;
    }

    #endregion

    #region Add

    /// <summary>
    /// Ajoute un �l�ment � la collection.
    /// </summary>
    /// <param name="item">L'�l�ment entr�.</param>
    /// /// <exception cref="ArgumentException">Lev�e si une cl� avec le m�me nom existe d�j� dans la collection.</exception>
    public void AddToList(U key, T item)
    {
        if (!HasItem(key))
        {
            list.Add(item);
            dictionary.Add(key, item);
            ItemAdded?.Invoke(item);
        }
        else
        {
            throw new ArgumentException("Une cl� avec ce nom existe d�j� dans la collection.");
        }
    }

    public void AddItemToDictionary(U key, T item)
    {
        if (!HasItem(key))
        {
            dictionary.Add(key, item);
        }
    }

    /// <summary>
    /// Ajoute un �l�ment d�faut � la collection.
    /// </summary>
    /// <param name="key">La cl� associ�e � l'�l�ment par d�faut.</param>
    /// /// <exception cref="ArgumentException">Lev�e si une cl� avec le m�me nom existe d�j� dans la collection.</exception>
    public void AddItem(U key)
    {
        if (!HasItem(key))
        {
            T item = default;
            list.Add(item);
            dictionary.Add(key, item);
            ItemAdded?.Invoke(item);
        }
        else
        {
            throw new ArgumentException("Une cl� avec ce nom existe d�j� dans la collection.");
        }
    }

    /// <summary>
    /// Ajoute un nouvel �l�ment � la collection avec la cl� sp�cifi�e.
    /// </summary>
    /// <param name="key">La cl� de l'�l�ment � ajouter.</param>
    /// <param name="item">L'�l�ment � ajouter � la collection.</param>
    /// <exception cref="ArgumentException">Lev�e si une cl� avec le m�me nom existe d�j� dans la collection.</exception>
    public void AddItem(U key, T item)
    {
        if (!HasItem(key))
        {
            list.Add(item);
            dictionary.Add(key, item);
            ItemAdded?.Invoke(item); // D�clenche l'�v�nement ItemAdded pour notifier les abonn�s.
        }
        else
        {
            throw new ArgumentException("Une cl� avec ce nom existe d�j� dans la collection.");
        }
    }

    /// <summary>
    /// Ajoute une s�rie d'�l�ments � la collection avec des cl�s g�n�r�es � partir de la cl� sp�cifi�e.
    /// </summary>
    /// <param name="keys">Les cl�s des �l�ments ajout�s.</param>
    /// <param name="items">Les �l�ments � ajouter � la collection.</param>
    /// <exception cref="ArgumentException">Lev�e si une cl� avec le m�me nom existe d�j� dans la collection, ce qui emp�che de cr�er une succession de cl�s � partir de ce nom.</exception>
    public void AddItems(IEnumerable<U> keys, IEnumerable<T> items)
    {
        int index = 0;
        foreach (T item in items)
        {
            if (!HasItem(keys.ElementAtOrDefault(index)))
            {
                list.Add(item);
                dictionary.Add(keys.ElementAtOrDefault(index), item);
                ItemAdded?.Invoke(item);
            }
            else
            {
                throw new ArgumentException("Une cl� avec ce nom existe d�j� dans la collection, impossible de cr�er une succession de cl� � partir de ce nom.");
            }
            index++;
        }
    }

    #endregion

    #region Getter

    /// <summary>
    /// R�cup�re le nom de la collection.
    /// </summary>
    /// <returns>Le nom de la collection.</returns>
    public string GetCollectionName() { return collectionName; }

    /// <summary>
    /// R�cup�re la cl� associ�e � l'�l�ment sp�cifi� dans la collection.
    /// </summary>
    /// <param name="item">L'�l�ment dont la cl� doit �tre r�cup�r�e.</param>
    /// <returns>La cl� associ�e � l'�l�ment, ou null si aucun �l�ment correspondant n'est trouv�.</returns>
    public U GetKeyByItem(T item)
    {
        foreach (var kvp in dictionary)
            if (EqualityComparer<T>.Default.Equals(kvp.Value, item)) return kvp.Key;
        return default(U);
    }

    /// <summary>
    /// R�cup�re l'�l�ment associ� � la cl� sp�cifi�e dans la collection.
    /// </summary>
    /// <param name="key">La cl� de l'�l�ment � r�cup�rer.</param>
    /// <returns>
    /// L'�l�ment associ� � la cl� sp�cifi�e s'il est trouv� dans la collection,
    /// sinon la valeur par d�faut de type T.
    /// </returns>
    public T GetItemBykey(U key)
    {
        if (HasItem(key)) return dictionary[key];
        return default(T);
    }

    /// <summary>
    /// R�cup�re l'�l�ment � l'index sp�cifi� dans la collection.
    /// </summary>
    /// <param name="index">L'index de l'�l�ment � r�cup�rer.</param>
    /// <returns>
    /// L'�l�ment � l'index sp�cifi� dans la collection, ou la valeur par d�faut de type T si l'index est invalide.
    /// </returns>
    public T GetItemByIndex(int index)
    {
        if (IsOutOfIndex(index)) return default(T);
        return list[index];
    }

    /// <summary>
    /// R�cup�re une liste contenant tous les �l�ments de la collection.
    /// </summary>
    /// <returns>Une liste contenant tous les �l�ments de la collection.</returns>
    public List<T> GetItemsList() { return list; }

    /// <summary>
    /// R�cup�re un dictionnaire contenant toutes les cl�s et leurs �l�ments correspondants de la collection.
    /// </summary>
    /// <returns>Un dictionnaire contenant toutes les cl�s et leurs �l�ments correspondants de la collection.</returns>
    public SerializableDictionary<U, T> GetItemsDictionary() { return dictionary; }

    /// <summary>
    /// R�cup�re le nombre d'�l�ments dans la collection.
    /// </summary>
    /// <returns>Le nombre d'�l�ments dans la collection.</returns>
    public int GetItemsNumber() { return list.Count; }

    #endregion

    #region Find

    /// <summary>
    /// Recherche et r�cup�re l'�l�ment associ� � la cl� sp�cifi�e dans la collection.
    /// </summary>
    /// <param name="key">La cl� de l'�l�ment � rechercher.</param>
    /// <returns>
    /// L'�l�ment associ� � la cl� sp�cifi�e s'il est trouv� dans la collection,
    /// sinon la valeur par d�faut de type T.
    /// </returns>
    public T FindItemBykey(U key)
    {
        if (HasItem(key)) return dictionary[key];
        return default(T);
    }

    /// <summary>
    /// Recherche et r�cup�re l'�l�ment � l'index sp�cifi� dans la collection.
    /// </summary>
    /// <param name="index">L'index de l'�l�ment � rechercher.</param>
    /// <returns>
    /// L'�l�ment � l'index sp�cifi� dans la collection,
    /// ou la valeur par d�faut de type T si l'index est invalide.
    /// </returns>
    public T FindItemByIndex(int index)
    {
        if (IsInOfIndex(index)) return list[index];
        return default(T);
    }

    /// <summary>
    /// Recherche et r�cup�re la cl� associ�e � l'�l�ment sp�cifi� dans la collection.
    /// </summary>
    /// <param name="item">L'�l�ment dont la cl� doit �tre recherch�e.</param>
    /// <returns>
    /// La cl� associ�e � l'�l�ment sp�cifi�, ou null si aucun �l�ment correspondant n'est trouv�.
    /// </returns>
    public U FindKeyByItem(T item)
    {
        U keyToFind = default(U);
        foreach (var kvp in dictionary)
        {
            if (EqualityComparer<T>.Default.Equals(kvp.Value, item))
            {
                keyToFind = kvp.Key;
                break;
            }
        }
        return keyToFind;
    }

    /// <summary>
    /// Recherche et r�cup�re tous les �l�ments de la collection qui correspondent au pr�dicat sp�cifi�.
    /// </summary>
    /// <param name="predicate">Le pr�dicat utilis� pour d�terminer les �l�ments � r�cup�rer.</param>
    /// <returns>Une liste contenant tous les �l�ments de la collection qui correspondent au pr�dicat sp�cifi�.</returns>
    public List<T> FindItems(Predicate<T> predicate)
    {
        return list.FindAll(predicate);
    }

    /// <summary>
    /// Recherche et r�cup�re tous les �l�ments de la collection qui satisfont la condition sp�cifi�e.
    /// </summary>
    /// <param name="condition">La fonction qui sp�cifie la condition � v�rifier pour chaque �l�ment.</param>
    /// <returns>Une liste contenant tous les �l�ments de la collection qui satisfont la condition sp�cifi�e.</returns>
    public List<T> FindItems(Func<T, bool> condition)
    {
        return list.Where(condition).ToList();
    }

    /// <summary>
    /// Recherche et r�cup�re tous les �l�ments de la collection qui ont une propri�t� sp�cifique �gale � une valeur donn�e.
    /// </summary>
    /// <param name="propertySelector">La fonction de s�lection de propri�t� utilis�e pour extraire la propri�t� sp�cifique de chaque �l�ment.</param>
    /// <param name="value">La valeur � comparer avec la propri�t� sp�cifique des �l�ments.</param>
    /// <returns>Une liste contenant tous les �l�ments de la collection dont la propri�t� sp�cifique est �gale � la valeur donn�e.</returns>
    public List<T> FindItemsByProperty(Func<T, object> propertySelector, object value)
    {
        return list.Where(item => propertySelector(item).Equals(value)).ToList();
    }

    /// <summary>
    /// Recherche et r�cup�re tous les �l�ments de la collection dont la propri�t� sp�cifi�e contient une correspondance partielle avec un motif donn�.
    /// </summary>
    /// <param name="propertySelector">La fonction de s�lection de propri�t� utilis�e pour extraire la propri�t� sp�cifique de chaque �l�ment.</param>
    /// <param name="pattern">Le motif � rechercher dans la propri�t� sp�cifique des �l�ments.</param>
    /// <returns>Une liste contenant tous les �l�ments de la collection dont la propri�t� sp�cifi�e contient une correspondance partielle avec le motif donn�.</returns>
    public List<T> FindItemsByPartialMatch(Func<T, string> propertySelector, string pattern)
    {
        return list.Where(item => propertySelector(item).Contains(pattern)).ToList();
    }

    /// <summary>
    /// Recherche et r�cup�re tous les �l�ments de la collection dont la propri�t� sp�cifi�e a une valeur minimale �gale ou sup�rieure � une valeur donn�e.
    /// </summary>
    /// <param name="propertySelector">La fonction de s�lection de propri�t� utilis�e pour extraire la propri�t� sp�cifique de chaque �l�ment.</param>
    /// <param name="minValue">La valeur minimale � comparer avec la propri�t� sp�cifique des �l�ments.</param>
    /// <returns>Une liste contenant tous les �l�ments de la collection dont la propri�t� sp�cifi�e a une valeur minimale �gale ou sup�rieure � la valeur donn�e.</returns>
    public List<T> FindItemsByMinValue(Func<T, int> propertySelector, int minValue)
    {
        return list.Where(item => propertySelector(item) >= minValue).ToList();
    }

    /// <summary>
    /// Recherche et r�cup�re tous les �l�ments de la collection dont la propri�t� sp�cifi�e a une valeur maximale �gale ou inf�rieure � une valeur donn�e.
    /// </summary>
    /// <param name="propertySelector">La fonction de s�lection de propri�t� utilis�e pour extraire la propri�t� sp�cifique de chaque �l�ment.</param>
    /// <param name="maxValue">La valeur maximale � comparer avec la propri�t� sp�cifique des �l�ments.</param>
    /// <returns>Une liste contenant tous les �l�ments de la collection dont la propri�t� sp�cifi�e a une valeur maximale �gale ou inf�rieure � la valeur donn�e.</returns>
    public List<T> FindItemsByMaxValue(Func<T, int> propertySelector, int maxValue)
    {
        return list.Where(item => propertySelector(item) <= maxValue).ToList();
    }

    #endregion

    #region Setter

    /// <summary>
    /// D�finit le nom de la collection.
    /// </summary>
    /// <param name="value">Le nouveau nom de la collection.</param>
    public void SetCollectionName(string value) { collectionName = value; }

    #endregion

    #region Remove

    /// <summary>
    /// Supprime l'�l�ment associ� � la cl� sp�cifi�e de la collection.
    /// </summary>
    /// <param name="key">La cl� de l'�l�ment � supprimer.</param>
    /// <exception cref="ArgumentException">Lev�e si aucune cl� correspondante n'est trouv�e dans la collection.</exception>
    public void RemoveItemBykey(U key)
    {
        if (HasItem(key))
        {
            T item = FindItemBykey(key);
            list.Remove(item);
            dictionary.Remove(key);
            ItemRemoved?.Invoke(item);
        }
        else
        {
            throw new ArgumentException("Une cl� avec ce nom n'existe pas dans la collection.");
        }
    }

    /// <summary>
    /// Supprime l'�l�ment sp�cifi� de la collection.
    /// </summary>
    /// <param name="item">L'�l�ment � supprimer de la collection.</param>
    /// <exception cref="ArgumentException">Lev�e si l'�l�ment sp�cifi� n'est pas pr�sent dans la collection.</exception>
    public void RemoveItemBySelf(T item)
    {
        if (HasItem(item))
        {
            list.Remove(item);
            dictionary.Remove(FindKeyByItem(item));
            ItemRemoved?.Invoke(item);
        }
        else
        {
            throw new ArgumentException("Cet item n'existe pas dans la collection.");
        }
    }

    /// <summary>
    /// Supprime tous les �l�ments sp�cifi�s de la collection.
    /// </summary>
    /// <param name="items">Une collection d'�l�ments � supprimer de la collection.</param>
    /// <remarks>
    /// Cette m�thode parcourt la liste des �l�ments sp�cifi�s et supprime chaque �l�ment de la collection.
    /// </remarks>
    public void RemoveItemsBySelf(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            RemoveItemBySelf(item);
        }
    }

    /// <summary>
    /// Supprime tous les �l�ments associ�s aux cl�s sp�cifi�es de la collection.
    /// </summary>
    /// <param name="keys">Une collection de cl�s � supprimer de la collection.</param>
    /// <remarks>
    /// Cette m�thode parcourt la liste des cl�s sp�cifi�es et supprime chaque �l�ment associ� � chaque cl� de la collection.
    /// </remarks>
    public void RemoveItemsByKey(IEnumerable<U> keys)
    {
        foreach (var key in keys)
        {
            RemoveItemBykey(key);
        }
    }

    #endregion

    #region Clear

    /// <summary>
    /// Supprime tous les �l�ments de la collection.
    /// </summary>
    /// <remarks>
    /// Cette m�thode vide � la fois la liste et le dictionnaire de la collection.
    /// Elle d�clenche �galement l'�v�nement <see cref="CollectionCleared"/> apr�s avoir vid� la collection.
    /// </remarks>
    public void ClearItems()
    {
        list.Clear();
        dictionary.Clear();
        CollectionCleared?.Invoke();
    }

    #endregion

    #region Save

    /// <summary>
    /// S�rialise la liste d'�l�ments de la collection et les sauvegarde dans un fichier binaire sp�cifi�.
    /// </summary>
    /// <param name="filePath">Le chemin d'acc�s complet du fichier de sauvegarde.</param>
    public void Save(string filePath)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            formatter.Serialize(stream, list);
        }
    }

    #endregion

    #region Load

    /// <summary>
    /// Charge une liste d'�l�ments � partir d'un fichier binaire sp�cifi� et les assigne � la collection.
    /// </summary>
    /// <param name="filePath">Le chemin d'acc�s complet du fichier de sauvegarde.</param>
    public void Load(string filePath)
    {
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                list = (List<T>)formatter.Deserialize(stream);
            }
        }
        else
        {
            Debug.LogError("Le fichier de sauvegarde n'existe pas.");
        }
    }

    #endregion

    public void OnDestroy()
    {
        ClearItems();
    }
}