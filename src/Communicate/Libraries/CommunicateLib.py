import clr
import types
import sys, os
sys.path.append(os.path.join(os.path.dirname(__file__), '..', '..','SharedLib', 'netcoreapp3.0'))
clr.AddReference('DotNetCore_RobotFrameWork.Communicate.WebApp')
from DotNetCore_RobotFrameWork.Communicate.WebApp import KeyWords

class CommunicateLib(object):

    def __init__(self, connection_string):
        self.Communicate_Lib = KeyWords(connection_string)

    def dispose_web_driver(self): 
        self.Communicate_Lib.dispose_web_driver()

    def open_app(self, browser_name = 'Chrome', url = ''): 
        self.Communicate_Lib.open_app(str(browser_name), str(url))

    def close_app(self): 
        self.Communicate_Lib.close_app()

    def navigate(self, url): 
        self.Communicate_Lib.navigate(str(url))

    def get_text(self, locator, timeout = -1, scroll = False): 
        return self.Communicate_Lib.get_text(str(locator), int(timeout), scroll)

    def input_text(self, locator, expected_text_or_key, ignore_case = True, timeout = -1, scroll = False): 
        return self.Communicate_Lib.input_text(str(locator), str(expected_text_or_key), ignore_case, int(timeout), scroll)

    def click(self, locator, timeout = -1, scroll = False): 
        return self.Communicate_Lib.click(str(locator), int(timeout), scroll)

    def equals_text(self, locator, expected_text_or_key, ignore_case = True, timeout = -1, scroll = False): 
        return self.Communicate_Lib.equals_text(str(locator), str(expected_text_or_key), ignore_case, int(timeout), scroll)

    def not_equals_text(self, locator, expected_text_or_key, ignore_case = True, timeout = -1, scroll = False): 
        return self.Communicate_Lib.not_equals_text(str(locator), str(expected_text_or_key), ignore_case, int(timeout), scroll)

    def contains_text(self, locator, expected_text_or_key, ignore_case = True, timeout = -1, scroll = False): 
        return self.Communicate_Lib.contains_text(str(locator), str(expected_text_or_key), ignore_case, int(timeout), scroll)

    def not_contains_text(self, locator, expected_text_or_key, ignore_case = True, timeout = -1, scroll = False): 
        return self.Communicate_Lib.not_contains_text(str(locator), str(expected_text_or_key), ignore_case, int(timeout), scroll)

    def press_a_key(self, locator, key_name, ignore_case = True, timeout = -1, scroll = False): 
        self.Communicate_Lib.press_a_key(str(locator), str(key_name), ignore_case, int(timeout), scroll)

    def has_visisble(self, locator, timeout = -1, scroll = False): 
        return self.Communicate_Lib.has_visisble(str(locator), int(timeout), scroll)

    def has_enable(self, locator, timeout = -1, scroll = False): 
        return self.Communicate_Lib.has_enable(str(locator), int(timeout), scroll)

    def scroll_to_view(self, locator, timeout = -1, scroll = False): 
        self.Communicate_Lib.scroll_to_view(str(locator), int(timeout), scroll)

    def count(self, locator, timeout = -1, scroll = False): 
        return self.Communicate_Lib.count(str(locator), int(timeout), scroll)

    def load_test_data(self, path_file, sheet_name, test_data_type): 
        self.Communicate_Lib.load_test_data(str(path_file), str(sheet_name), str(test_data_type))

    def start_test_case(self, test_caseId): 
        self.Communicate_Lib.start_test_case(str(test_caseId))

    def get_test_data_value_by_key(self, key): 
        return self.Communicate_Lib.get_test_data_value_by_key(str(key))

    def switch_connection_string(self, connection_string): 
        self.Communicate_Lib.switch_connection_string(str(connection_string))

    def get_total_row(self, commandText): 
        return self.Communicate_Lib.get_total_row(str(commandText))

    def get_json_data(self, commandText): 
        return self.Communicate_Lib.get_json_data(str(commandText))

    def get_single_value(self, commandText): 
        return self.Communicate_Lib.get_single_value(str(commandText))

    def execute_sql_command(self, commandText): 
        self.Communicate_Lib.execute_sql_command(str(commandText))

    def compare_2_results(self, commandText1, commandText2): 
        return self.Communicate_Lib.compare_2_results(str(commandText1), str(commandText2))

    def get_data_in_cell(self, locator, row_index, column_name, timeout = -1, scroll = False): 
        return self.Communicate_Lib.get_data_in_cell(str(locator), int(row_index), str(column_name), int(timeout), scroll)

    def verify_data_in_cell(self, locator, expected_text_or_key, row_index, column_name, ignore_case = True, timeout = -1, scroll = False): 
        return self.Communicate_Lib.verify_data_in_cell(str(locator), str(expected_text_or_key), int(row_index), str(column_name), ignore_case, int(timeout), scroll)

