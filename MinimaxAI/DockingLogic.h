#pragma once
#ifdef DLL_IMPORT_API_EXPORTS
#define DLL_IMPORT_API __declspec(dllexport) 
#else
#define DLL_IMPORT_API __declspec(dllimport) 
#endif


extern "C"
{


}
