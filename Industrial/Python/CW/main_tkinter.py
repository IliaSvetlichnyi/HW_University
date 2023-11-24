import tkinter as tk
from tkinter import filedialog, messagebox
import json
import pandas as pd
from views_by_country import views_by_country

# Global variable to hold the DataFrame
df = None


def open_file():
    global df  # Declare 'df' as global to modify the global variable
    file_path = filedialog.askopenfilename(
        filetypes=[("JSON files", "*.json")])
    if file_path:
        try:
            with open(file_path, 'r') as file:
                data = [json.loads(line) for line in file]
            df = pd.DataFrame(data)  # Assign data to 'df'
            # Call function to display task buttons
            display_task_buttons(file_path)
        except json.JSONDecodeError as e:
            messagebox.showerror("Error", f"Error loading JSON: {e}")


def display_json_data(file_path, data):
    new_window = tk.Toplevel()
    new_window.title(f"File: {file_path}")

    # Display the JSON data
    text_widget = tk.Text(new_window, height=20, width=100)
    text_widget.insert(tk.END, "\n\n".join(
        json.dumps(item, indent=4) for item in data))
    text_widget.pack()

    # Display the buttons for each task
    button_frame = tk.Frame(new_window)
    button_frame.pack(pady=10)

    for task_name in task_functions.keys():
        button = tk.Button(button_frame, text=task_name,
                           command=lambda name=task_name: open_id_document_window(name))
        button.pack(side=tk.LEFT, padx=5)

def display_task_buttons(file_path):
    task_window = tk.Toplevel()
    task_window.title(f"Tasks for file: {file_path}")

    # Display the buttons for each task
    button1 = tk.Button(task_window, text="Task 1",
                        command=open_id_document_window)
    button1.pack(side=tk.LEFT, padx=5)
    # ... create other buttons for other tasks


# Global variable to store the document ID for different tasks
document_id_for_tasks = {}

def open_id_document_window(task_name):
    id_document_window = tk.Toplevel()
    id_document_window.title("ID Document")

    input_label = tk.Label(id_document_window, text="Enter ID Document:")
    input_label.pack(pady=10)

    entry = tk.Entry(id_document_window)
    entry.pack(pady=5)

    ok_button = tk.Button(id_document_window, text="OK",
                          command=lambda: store_doc_id_and_execute(entry.get(), task_name))
    ok_button.pack(pady=10)


def store_doc_id_and_execute(doc_id, task_name):
    # Store the document ID for the task
    document_id_for_tasks[task_name] = doc_id
    # Call the appropriate function based on the task_name
    if task_name in task_functions:
        # You need to define these functions
        task_functions[task_name](df, doc_id)
    else:
        messagebox.showinfo(
            "Info", f"No function defined for task {task_name}")


# Dictionary mapping task names to functions
task_functions = {
    "Task 1": views_by_country,
    # "Task 2": other_task_function,  # Define other tasks functions similarly
    # "Task 3": yet_another_task_function,
}

def views_by_country_command(document_id):
    # This function gets called when the user inputs a document ID and clicks OK
    if df is not None:
        # Assuming views_by_country function handles the plotting
        views_by_country(df, document_id)
    else:
        messagebox.showinfo(
            "Info", "No data loaded. Please upload a JSON file first.")


def create_app():
    root = tk.Tk()
    root.title("JSON File Uploader")

    label = tk.Label(root, text="Choose a JSON file to upload")
    label.pack(pady=10)

    upload_button = tk.Button(root, text="Upload File", command=open_file)
    upload_button.pack(pady=100, padx=100)

    root.mainloop()


if __name__ == "__main__":
    create_app()
