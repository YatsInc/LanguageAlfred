#pragma once

#include <Windows.h>

class LangSwitch {
public:
	bool SwitchLang( DWORD kbLayout);
};

extern "C" __declspec(dllexport) bool SwitchLang(DWORD kbLayout) {
	LangSwitch ls;
	return ls.SwitchLang(kbLayout);
}