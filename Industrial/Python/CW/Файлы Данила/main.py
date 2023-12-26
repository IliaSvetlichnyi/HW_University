import tkinter as tk
import json
import pandas as pd
import argparse
import unittest

from tkinter import messagebox
from views_by_country import views_by_country
from views_by_continent import views_by_continent
from views_by_browser_clear import views_by_browser_clear
from views_by_visitor_useragent import views_by_visitor_useragent
from reader_profile import show_reader_profile
from tkinter import filedialog
from also_like_graph import create_graph, upload_data, sort_by_readers


id = "140228202800-6ef39a241f35301a9a42cd0ed21e5fb0"
id2 = "130711094717-71ef362ab87870b23411460a3989221a"

def upload_file():
    json_data = []
    file_path = filedialog.askopenfilename(filetypes=[("JSON files", "*.json")])
    if file_path:  
        with open(file_path, 'r') as file:
            for line in file:
                try:
                    json_object = json.loads(line)
                    json_data.append(json_object)
                except json.JSONDecodeError:
                    print("Error decoding JSON from line:", line)
    if json_data:
        df = pd.DataFrame(json_data)
        display_json_data(file_path, json_data, df)
    else:
        print("No valid JSON data found in the file.")


def display_json_data(file_path, data, df):
    new_window = tk.Toplevel()
    new_window.title(f"File: {file_path}")

    label = tk.Label(new_window, text="JSON Data:", font=("Arial", 12, "bold"))
    label.pack(pady=20)

    text_widget = tk.Text(new_window, height=20, width=100)
    text_widget.insert(tk.END, "\n\n".join(json.dumps(item, indent=4) for item in data))
    text_widget.pack()

    button_frame = tk.Frame(new_window)
    button_frame.pack(pady=10)

    button1 = tk.Button(button_frame, text="Views by country/continent", command=lambda: button_action(1, df, file_path))
    button1.pack(side=tk.LEFT, padx=5)

    button2 = tk.Button(button_frame, text="Views by browser", command=lambda: button_action(2, df, file_path))
    button2.pack(side=tk.LEFT, padx=5)

    button3 = tk.Button(button_frame, text="Reader profiles", command=lambda: button_action(3, df, file_path))
    button3.pack(side=tk.LEFT, padx=5)

    button4 = tk.Button(button_frame, text="Also likes", command=lambda: button_action(4, df, file_path))
    button4.pack(side=tk.LEFT, padx=5)


def button_action(button_number, df, file_path):
    if button_number == 1:
        id_document_window = tk.Toplevel()
        id_document_window.title("ID Document")

        input_label = tk.Label(id_document_window, text="Views by country:")
        input_label.pack(pady=20)

        entry1 = tk.Entry(id_document_window)
        entry1.pack(pady=5)

        ok_button1 = tk.Button(id_document_window, text="OK", command=lambda: views_by_country(df, entry1.get()))
        ok_button1.pack(pady=10)
        
        input_label = tk.Label(id_document_window, text="Views by continent:")
        input_label.pack(pady=20)
        
        entry2 = tk.Entry(id_document_window)
        entry2.pack(pady=5)
        
        ok_button2 = tk.Button(id_document_window, text="OK", command=lambda: views_by_continent(df, entry2.get()))
        ok_button2.pack(pady=10)
        
    elif button_number == 2:
        id_document_window = tk.Toplevel()
        id_document_window.title("ID Document")
          
        input_label = tk.Label(id_document_window, text="All browser identifiers of the viewers:")
        input_label.pack(pady=20)

        entry1 = tk.Entry(id_document_window)
        entry1.pack(pady=5)
          
        ok_button1 = tk.Button(id_document_window, text="OK", command=lambda: views_by_visitor_useragent(df, entry1.get()))
        ok_button1.pack(pady=10)

        input_label = tk.Label(id_document_window, text="Main browser:")
        input_label.pack(pady=20)

        entry2 = tk.Entry(id_document_window)
        entry2.pack(pady=5)
          
        ok_button2 = tk.Button(id_document_window, text="OK", command=lambda: views_by_browser_clear(df, entry2.get()))
        ok_button2.pack(pady=10)
    
    elif button_number == 3:
        id_document_window = tk.Toplevel()
        id_document_window.title("ID Document")
          
        input_label = tk.Label(id_document_window, text="Reader profiles:")
        input_label.pack(pady=20)

        entry1 = tk.Entry(id_document_window)
        entry1.pack(pady=5)


        ok_button1 = tk.Button(id_document_window, text="OK", command=lambda: show_reader_profile(df, entry1.get()))
        ok_button1.pack(pady=10)

    elif button_number == 4:
        id_document_window = tk.Toplevel()
        id_document_window.title("ID Document")
          
        input_label = tk.Label(id_document_window, text="Also likes:")
        input_label.pack(pady=20)

        entry1 = tk.Entry(id_document_window)
        entry1.pack(pady=5)

        ok_button1 = tk.Button(id_document_window, text="OK", command=lambda: create_graph(entry1.get(), upload_data(file_path)))
        ok_button1.pack(pady=10)

        input_label = tk.Label(id_document_window, text="Top 10 docs:")
        input_label.pack(pady=20)

        ok_button2 = tk.Button(id_document_window, text="OK", command=lambda: sort_by_readers(entry1.get(), df))
        ok_button2.pack(pady=10)


def create_app():
    root = tk.Tk()
    root.title("JSON File Uploader")

    label = tk.Label(root, text="Choose a JSON file to upload")
    label.pack(pady=10)

    upload_button = tk.Button(root, text="Upload File", command=upload_file)
    upload_button.pack(pady=100, padx=100)

    root.minsize(300, 200) 
    root.maxsize(500, 400)

    root.mainloop()
    

def main():
    parser = argparse.ArgumentParser(description='JSON File Analysis Tool')
    parser.add_argument('-u', '--user_uuid', help='User UUID', required=True)
    parser.add_argument('-d', '--doc_uuid', help='Document UUID', required=True)
    parser.add_argument('-t', '--task_id', help='Task ID', required=True)
    parser.add_argument('-f', '--file_name', help='File Name', required=True)
    
    args = parser.parse_args()

    print(f"User UUID: {args.user_uuid}")
    print(f"Document UUID: {args.doc_uuid}")
    print(f"Task ID: {args.task_id}")
    print(f"File Name: {args.file_name}")

    json_data = []
    file_path = args.file_name
    if file_path:  
        with open(file_path, 'r') as file:
            for line in file:
                try:
                    json_object = json.loads(line)
                    json_data.append(json_object)
                except json.JSONDecodeError:
                    print("Error decoding JSON from line:", line)
    if json_data:
        df = pd.DataFrame(json_data)
        display_json_data(file_path, json_data, df)
    else:
        print("No valid JSON data found in the file.")

    if args.task_id == '2a':
        views_by_country(df, args.doc_uuid)
       
    elif args.task_id == '2b':
        views_by_continent(df, args.doc_uuid)
        
    elif args.task_id == '3a':
        views_by_visitor_useragent(df, args.doc_uuid)
        
    elif args.task_id == '3b':
        views_by_browser_clear(df, args.doc_uuid)

    elif args.task_id == '4':
        show_reader_profile(df, args.doc_uuid)
    
    elif args.task_id == '5d':
         sort_by_readers (df, args.doc_uuid)
     
    elif args.task_id == '6':
         create_graph(args.doc_uuid, upload_data(args.file_name))
    
    elif args.task_id == '7':
         create_graph(args.doc_uuid, upload_data(args.file_name))


if __name__ == "__main__":
    import sys

    if len(sys.argv) > 1:
 
        main()
        unittest.main()
    else:
        
        create_app()


class TestMainFunctionality(unittest.TestCase):

    def test_args_2a(self):
        args = ['-u', 'dan', '-d', '140228202800-6ef39a241f35301a9a42cd0ed21e5fb0', '-t', '2a', '-f', 'sample_small.json']
        with unittest.mock.patch('sys.argv', ['main.py'] + args):
            main()
           

    def test_args_4(self):
        args = ['-u', 'dan', '-d', 'some_doc_uuid', '-t', '4', '-f', 'sample_small.json']
        with unittest.mock.patch('sys.argv', ['main.py'] + args):
            main()
            

if __name__ == '__main__':
    unittest.main()