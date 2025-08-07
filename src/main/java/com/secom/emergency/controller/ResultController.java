package com.secom.emergency.controller;

import com.secom.emergency.entity.TerminalSimHimoduke;
import com.secom.emergency.service.TerminalSimHimodukeService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.servlet.mvc.support.RedirectAttributes;

@Controller
@RequestMapping("/result")
public class ResultController {
    
    @Autowired
    private TerminalSimHimodukeService himodukeService;
    
    private static final int PAGE_SIZE = 20; // 20件/ページ
    
    /**
     * SIM情報紐付け結果一覧画面を表示
     */
    @GetMapping
    public String result(@RequestParam(defaultValue = "0") int page, Model model) {
        
        // 当日登録分のデータを取得（ページング対応）
        Page<TerminalSimHimoduke> himodukePage = himodukeService.getTodayRegistrations(page, PAGE_SIZE);
        
        model.addAttribute("himodukePage", himodukePage);
        model.addAttribute("currentPage", page);
        model.addAttribute("totalPages", himodukePage.getTotalPages());
        model.addAttribute("totalElements", himodukePage.getTotalElements());
        model.addAttribute("pageSize", PAGE_SIZE);
        
        // ページング用の情報
        model.addAttribute("hasPrevious", himodukePage.hasPrevious());
        model.addAttribute("hasNext", himodukePage.hasNext());
        model.addAttribute("previousPage", page > 0 ? page - 1 : 0);
        model.addAttribute("nextPage", page + 1);
        
        return "result";
    }
    
    /**
     * 紐付けデータ削除処理
     */
    @PostMapping("/delete")
    public String deleteHimoduke(@RequestParam("id") Long id,
                                @RequestParam(defaultValue = "0") int page,
                                RedirectAttributes redirectAttributes) {
        
        boolean deleted = himodukeService.deleteHimoduke(id);
        
        if (deleted) {
            redirectAttributes.addFlashAttribute("deleteSuccess", 
                "紐付けデータを削除しました。");
        } else {
            redirectAttributes.addFlashAttribute("deleteError", 
                "削除対象のデータが見つかりません。");
        }
        
        // 削除後は同じページに戻る（ただし、データがなくなった場合は前のページに）
        redirectAttributes.addAttribute("page", page);
        return "redirect:/result";
    }
    
    /**
     * 削除確認用AJAX処理
     */
    @GetMapping("/confirm-delete/{id}")
    @ResponseBody
    public TerminalSimHimoduke confirmDelete(@PathVariable Long id) {
        return himodukeService.findById(id);
    }
}