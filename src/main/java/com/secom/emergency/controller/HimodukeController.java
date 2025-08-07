package com.secom.emergency.controller;

import com.secom.emergency.service.TerminalSimHimodukeService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.servlet.mvc.support.RedirectAttributes;

import java.util.List;

@Controller
@RequestMapping("/himoduke")
public class HimodukeController {
    
    @Autowired
    private TerminalSimHimodukeService himodukeService;
    
    /**
     * 端末-SIM情報紐付け登録画面を表示
     */
    @GetMapping
    public String himoduke() {
        return "himoduke";
    }
    
    /**
     * 端末-SIM紐付け登録処理
     */
    @PostMapping("/register")
    public String registerHimoduke(@RequestParam("imei") String imei,
                                  @RequestParam("iccid") String iccid,
                                  RedirectAttributes redirectAttributes) {
        
        // 入力値の前後空白を除去
        imei = imei != null ? imei.trim() : "";
        iccid = iccid != null ? iccid.trim() : "";
        
        List<String> errors = himodukeService.registerHimoduke(imei, iccid);
        
        if (errors.isEmpty()) {
            redirectAttributes.addFlashAttribute("success", 
                "端末-SIM紐付けデータの登録が完了しました。");
        } else {
            redirectAttributes.addFlashAttribute("errors", errors);
            redirectAttributes.addFlashAttribute("inputImei", imei);
            redirectAttributes.addFlashAttribute("inputIccid", iccid);
        }
        
        return "redirect:/himoduke";
    }
}