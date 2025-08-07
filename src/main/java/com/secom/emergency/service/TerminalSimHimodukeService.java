package com.secom.emergency.service;

import com.secom.emergency.entity.TerminalSimHimoduke;
import com.secom.emergency.repository.TerminalSimHimodukeRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.List;

@Service
@Transactional
public class TerminalSimHimodukeService {
    
    @Autowired
    private TerminalSimHimodukeRepository himodukeRepository;
    
    /**
     * 端末-SIM紐付けデータを登録
     */
    public List<String> registerHimoduke(String imei, String iccid) {
        List<String> errorMessages = new ArrayList<>();
        
        // 入力値バリデーション
        List<String> validationErrors = validateHimodukeData(imei, iccid);
        if (!validationErrors.isEmpty()) {
            return validationErrors;
        }
        
        // 重複チェック
        if (himodukeRepository.existsByImeiAndIccid(imei, iccid)) {
            errorMessages.add("この端末製造番号(IMEI)とICカード番号(ICCID)の組み合わせは既に登録されています。");
            return errorMessages;
        }
        
        try {
            // データベース登録
            TerminalSimHimoduke himoduke = new TerminalSimHimoduke(imei, iccid);
            himodukeRepository.save(himoduke);
        } catch (Exception e) {
            errorMessages.add("データベース登録でエラーが発生しました: " + e.getMessage());
        }
        
        return errorMessages;
    }
    
    /**
     * 紐付けデータのバリデーション
     */
    private List<String> validateHimodukeData(String imei, String iccid) {
        List<String> errors = new ArrayList<>();
        
        // IMEI（端末製造番号）チェック（15桁の数字）
        if (imei == null || imei.trim().isEmpty()) {
            errors.add("端末製造番号(IMEI)は必須です。");
        } else if (!imei.matches("\\d{15}")) {
            errors.add("端末製造番号(IMEI)は15桁の数字で入力してください。");
        }
        
        // ICCID（ICカード番号）チェック（19桁の数字）
        if (iccid == null || iccid.trim().isEmpty()) {
            errors.add("ICカード番号(ICCID)は必須です。");
        } else if (!iccid.matches("\\d{19}")) {
            errors.add("ICカード番号(ICCID)は19桁の数字で入力してください。");
        }
        
        return errors;
    }
    
    /**
     * 当日登録分の紐付けデータを取得（ページング対応）
     */
    @Transactional(readOnly = true)
    public Page<TerminalSimHimoduke> getTodayRegistrations(int page, int size) {
        LocalDateTime startOfDay = LocalDate.now().atStartOfDay();
        LocalDateTime endOfDay = LocalDate.now().atTime(LocalTime.MAX);
        
        Pageable pageable = PageRequest.of(page, size);
        return himodukeRepository.findTodayRegistrations(startOfDay, endOfDay, pageable);
    }
    
    /**
     * 当日登録分の件数を取得
     */
    @Transactional(readOnly = true)
    public long getTodayRegistrationCount() {
        LocalDateTime startOfDay = LocalDate.now().atStartOfDay();
        LocalDateTime endOfDay = LocalDate.now().atTime(LocalTime.MAX);
        
        return himodukeRepository.countTodayRegistrations(startOfDay, endOfDay);
    }
    
    /**
     * 紐付けデータを削除
     */
    public boolean deleteHimoduke(Long id) {
        try {
            if (himodukeRepository.existsById(id)) {
                himodukeRepository.deleteById(id);
                return true;
            }
            return false;
        } catch (Exception e) {
            return false;
        }
    }
    
    /**
     * 全ての紐付けデータを取得
     */
    @Transactional(readOnly = true)
    public List<TerminalSimHimoduke> getAllHimoduke() {
        return himodukeRepository.findAll();
    }
    
    /**
     * IDで紐付けデータを取得
     */
    @Transactional(readOnly = true)
    public TerminalSimHimoduke findById(Long id) {
        return himodukeRepository.findById(id).orElse(null);
    }
}