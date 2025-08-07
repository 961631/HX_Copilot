package com.secom.emergency.controller;

import com.secom.emergency.service.SimRegistService;
import com.secom.emergency.service.TerminalRegistService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.multipart.MultipartFile;
import org.springframework.web.servlet.mvc.support.RedirectAttributes;

import java.util.List;

@Controller
@RequestMapping("/regist")
public class RegistController {
    
    @Autowired
    private TerminalRegistService terminalService;
    
    @Autowired
    private SimRegistService simService;
    
    /**
     * KDDI受領データ登録画面を表示
     */
    @GetMapping
    public String regist() {
        return "regist";
    }
    
    /**
     * 端末データCSVアップロード処理
     */
    @PostMapping("/terminal")
    public String uploadTerminalCsv(@RequestParam("terminalFile") MultipartFile file,
                                   RedirectAttributes redirectAttributes) {
        try {
            if (file.isEmpty()) {
                redirectAttributes.addFlashAttribute("terminalError", "ファイルが選択されていません。");
                return "redirect:/regist";
            }
            
            List<String> errors = terminalService.registerTerminalsFromCsv(file);
            
            if (errors.isEmpty()) {
                redirectAttributes.addFlashAttribute("terminalSuccess", 
                    "端末データの登録が完了しました。");
            } else {
                redirectAttributes.addFlashAttribute("terminalErrors", errors);
            }
            
        } catch (Exception e) {
            redirectAttributes.addFlashAttribute("terminalError", 
                "ファイル処理中にエラーが発生しました: " + e.getMessage());
        }
        
        return "redirect:/regist";
    }
    
    /**
     * SIMデータCSVアップロード処理
     */
    @PostMapping("/sim")
    public String uploadSimCsv(@RequestParam("simFile") MultipartFile file,
                              RedirectAttributes redirectAttributes) {
        try {
            if (file.isEmpty()) {
                redirectAttributes.addFlashAttribute("simError", "ファイルが選択されていません。");
                return "redirect:/regist";
            }
            
            List<String> errors = simService.registerSimsFromCsv(file);
            
            if (errors.isEmpty()) {
                redirectAttributes.addFlashAttribute("simSuccess", 
                    "SIMデータの登録が完了しました。");
            } else {
                redirectAttributes.addFlashAttribute("simErrors", errors);
            }
            
        } catch (Exception e) {
            redirectAttributes.addFlashAttribute("simError", 
                "ファイル処理中にエラーが発生しました: " + e.getMessage());
        }
        
        return "redirect:/regist";
    }
}