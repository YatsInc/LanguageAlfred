#include "pch.h"
#include "LangSwitch.h"

#include <iostream>

bool LangSwitch::SwitchLang(DWORD kbLayout)
{
	HWND hCon = (HWND)0xffff;
	bool msg1Res = PostMessage(hCon, WM_INPUTLANGCHANGEREQUEST, 0, (LPARAM)kbLayout);
	bool msg2Res = PostMessage(hCon, WM_INPUTLANGCHANGE, 0, (LPARAM)kbLayout);

	return msg1Res && msg2Res;
}
