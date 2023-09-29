// See https://aka.ms/new-console-template for more information
// Example demonstrating class hierarchies and inheritance
// From Lecture 3: C# Basics
// -----------------------------------------------------------------------------

namespace Week2{
// The base class: generic information on a person
class Person{
  // data fields for Person; uses a mixture of access modifiers for demonstration
  public string fName;
  private string lName;
  private string address;

  public Person(string fName, string lName, string address){
    this.fName = fName;
    this.lName = lName;
    this.address = address;
  }

  // explicit, public access methods to the data fields
  public string GetfName(){return fName;}
  public string GetlName(){return lName;}
  public string GetAddress(){return address;}

  public override string ToString() {
    return String.Format("Name: {0} {1}\tAddress: {2}", this.fName, this.lName, this.address);
  }
}

// Student is a sub-class of Person, and inherits its data-fields and methods
class Student: Person{
  // data fields for Student
  private string matricNo;
  private string degree;
	
  public Student(string fName, string lName, string address, string matricNo, string degree): base(fName, lName, address) {
    this.matricNo = matricNo;
    this.degree = degree;
  }
  
  // explicit, public access methods to the data fields
  public string GetMatricNo(){return matricNo;}
  public string GetDegree(){return degree;}

  // override ToString as an example of serialisation
  public override string ToString() {
    string base_str = base.ToString();
    string this_str = String.Format("MatricNo: {0}\tDegree: {1}", this.matricNo, this.degree);
    return base_str+"\n"+this_str;
  }
  // A different way to implement ToString; it is less generic, 
  // because it relies on knowledge of the field in all parent classes
  public string ToString0() {
    return String.Format("Name: {0} {1}\tAddress: {2}\nMatricNo: {3}\tDegree: {4}", 
			 this.GetfName(), this.GetlName(), this.GetAddress(), this.matricNo, this.degree);
  }

}

// Lecturer is another sub-class of Person, and inherits its data-fields and methods
class Lecturer: Person{
  // data fields for Lecturer
  private string officeNo;
  
  public Lecturer(string fName, string lName, string address, string officeNo): base(fName, lName, address) {
    this.officeNo = officeNo;
  }
  // explicit, public access methods to the data fields
  public string GetOfficeNo(){return officeNo;}
  // override ToString as an example of serialisation
  public override string ToString() {
    string base_str = base.ToString();
    string this_str = String.Format("OfficeNo: {0}", this.officeNo);
    return base_str+"\n"+this_str;
  }
}

class Test{
    public static void Main(){
      Person p = new Person("John", "Smith", "Edinburgh");
      Student s = new Student("Brian", "Hillman", "London", "99124678", "CS");
      Lecturer l = new Lecturer("Hans-Wolfgang", "Loidl", "Edinburgh", "G48");
      Console.WriteLine("\nInstantiating a person p, student s and lecturer l");
      Console.WriteLine("Person p: {0} ", p.ToString());
      Console.WriteLine("Student s: {0} ", s.ToString());
      Console.WriteLine("Lecturer l: {0} ", l.ToString());
      Console.WriteLine("\nBasic tests, showing values after instantiating basic objects:");
      Console.WriteLine("Student matric no: {0} ", s.GetMatricNo());
      Console.WriteLine("Student address: {0} ", s.GetAddress());
      Console.WriteLine("Person address: {0} ", p.GetAddress());
      Console.WriteLine("Lecturer address: {0} ", l.GetAddress());
      Console.WriteLine("Lecturer office: {0} ", l.GetOfficeNo());
      // ---
      
      Console.WriteLine("\nNow, copying the object and updating first name, demonstrating reference semantics in objects, ie. the update of first name in q affects p, too");
      Person q = p;
      Console.WriteLine("Person p: {0} ", p.ToString());
      Console.WriteLine("Person q: {0} ", q.ToString());
      q.fName = "Will";
      Console.WriteLine("Person p: {0} ", p.ToString());
      Console.WriteLine("Person q: {0} ", q.ToString());

      Console.WriteLine("\nHere we use the overriden ToString() method, implemented as a generic serialisation method:");
      Console.WriteLine("Student: {0} ", s.ToString0());
      Console.WriteLine("Student: {0} ", s.ToString());
      Console.WriteLine("Lecturer: {0} ", l.ToString());
    }
}

class Numbers{

    private static int x;
    private static int y;

    public Numbers(int x, int y){

        this.x = x;
        this.y = y;

    }

    public static void add(){
        Console.WriteLine(x + y);
    }
}

class LiveCoding{

    static void Main(){
        Lecturer lecturer = new Lecturer("Ali", "Mustide", "UAE", "5.11");
        Console.WriteLine(lecturer.ToString)
        // Numbers lc = new Numbers(2, 2);
        // lc.Set
        // Console.WriteLine(lc.Get());
        // Console.WriteLine(lc.name);
    }
}

}